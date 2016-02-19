using System;
using System.Threading;
using System.Threading.Tasks;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;

namespace External
{
    public delegate void ProcessAFDataPipeEventDelegate(AFDataPipeEvent evt);

    public class AFDataObserver : IObserver<AFDataPipeEvent>, IDisposable
    {
        public AFAttributeList AttributeList { get; set; }
        public AFDataPipe DataPipe { get; set; }

        private int _threadSleepTimeInMilliseconds;

        private ProcessAFDataPipeEventDelegate _processEvent;

        public AFDataObserver(AFAttributeList attrList, ProcessAFDataPipeEventDelegate processEvent, int pollInterval = 5000)
        {
            AttributeList = attrList;
            DataPipe = new AFDataPipe();
            _threadSleepTimeInMilliseconds = pollInterval;
            _processEvent = processEvent;
        }

        public void Start()
        {
            DataPipe.Subscribe(this);

            AFErrors<AFAttribute> errors = DataPipe.AddSignups(AttributeList);

            // This task loop calls GetObserverEvents every 5 seconds
            Task mainTask = Task.Run(() =>
            {
                bool hasMoreEvents = false;
                while (true)
                {
                    AFErrors<AFAttribute> results = DataPipe.GetObserverEvents(out hasMoreEvents);
                    if (!hasMoreEvents)
                        Thread.Sleep(_threadSleepTimeInMilliseconds);
                }
            });
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { throw error; }

        public void OnNext(AFDataPipeEvent dpEvent)
        {
            _processEvent(dpEvent);
        }

        public void Dispose()
        {
            DataPipe.Dispose();
            DataPipe = null;
        }
    }
}
