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
        // The list of attributes to monitor
        public AFAttributeList AttributeList { get; set; }

        // The underlying AFDataPipe that provides incoming values
        public AFDataPipe DataPipe { get; set; }

        // Interval to wait in between calling the data pipe
        private int _threadSleepTimeInMilliseconds;

        // The client provides this delegate to call during OnNext()
        private ProcessAFDataPipeEventDelegate _processEvent;

        public AFDataObserver(AFAttributeList attrList, ProcessAFDataPipeEventDelegate processEvent, int pollInterval = 5000)
        {
            AttributeList = attrList;
            DataPipe = new AFDataPipe();
            _threadSleepTimeInMilliseconds = pollInterval;
            _processEvent = processEvent;
        }

        public AFErrors<AFAttribute> Start()
        {
            // Subscribe this object (Observer) to the AFDataPipe (Observable)
            DataPipe.Subscribe(this);

            // The data pipe will provide updates from attributes inside AttributeList
            AFErrors<AFAttribute> errors = DataPipe.AddSignups(AttributeList);

            if (errors != null)
            {
                return errors;
            }
            else
            {
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
                return null;
            }
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { throw error; }

        public void OnNext(AFDataPipeEvent dpEvent)
        {
            // AFDataPipeEvent contains the AFValue representing the incoming event
            _processEvent(dpEvent);
        }

        public void Dispose()
        {
            DataPipe.Dispose();
            DataPipe = null;
        }
    }
}
