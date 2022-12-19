using EfCoreLab4;

ThreadsHandler handler = new(new ExampleContext());

//handler.LockExample();

//handler.MonitorExample();

//handler.AutoResetEventExample();

//handler.MutexExample();

handler.SemaphoreExample();
