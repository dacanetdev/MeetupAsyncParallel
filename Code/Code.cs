//var webClient = new WebClient();
var client = new HttpClient();

Recorder.Start();
//PrintUrls(urls);
//PrintUrlsWithThreads(urls);
//PrintUrlsWithThreadsBetter(urls);
//PrintUrlsWithThreadsMuchBetter(urls);
//await PrintUrlsWithThreadsAsyncParallelAsync(urls);
//await Task.WhenAll(PrintUrlsWithTasks(urls));
//await PrintUrlsAsync(urls);
//await PrintUrlsAsyncParallel(urls);
//await PrintUrlsAsyncParallelAsync(urls);
//await PrintUrlsAsyncParallelAsyncAsync(urls);
//await PrintUrlsAsyncParallelWithHttpClientAsync(urls);
Recorder.Stop();

ReadKey();

void PrintUrls(IEnumerable<string> urls)
{
	foreach (var url in urls)
	{
		ShowDownloadedData(url);
	}
}

void PrintUrlsWithThreads(IEnumerable<string> urls)
{
	foreach (var url in urls)
	{
		var tDownload = new Thread(new ParameterizedThreadStart(ShowDownloadedData));
		tDownload.Start(url);
	}
}

void PrintUrlsWithThreadsBetter(IEnumerable<string> urls)
{
	var threads = new List<Thread>();
	
	foreach (var url in urls)
	{
		var tDownload = new Thread(new ParameterizedThreadStart(ShowDownloadedData));
		tDownload.Start(url);
		threads.Add(tDownload);
	}

	foreach(var thread in threads)
    {
		thread.Join();
    }
}

async Task PrintUrlsWithThreadsAsyncParallelAsync(IEnumerable<string> urls)
{
	var threads = new List<Thread>();

	foreach (var url in urls)
	{
		var ts = new ThreadWithState(url);
		var tDownload = new Thread(new ThreadStart(ts.ShowDownloadedData));
		threads.Add(tDownload);
	}

	await Parallel.ForEachAsync(threads, async (thread, token) =>
	{
		await Task.Run(() =>
        {
			thread.Start();
			thread.Join();
        });
	});
}

void PrintUrlsWithThreadsMuchBetter(IEnumerable<string> urls)
{
	urls.Select(url =>
	{
		var tDownload = new Thread(new ParameterizedThreadStart(ShowDownloadedData));
		tDownload.Start(url);
		return tDownload;
	}).ToList().ForEach(t => t.Join());
}

IEnumerable<Task> PrintUrlsWithTasks(IEnumerable<string> urls)
{
	var tasks = new List<Task>();
	foreach (var url in urls)
	{
		tasks.Add(Task.Run(() => ShowDownloadedData(url)));
	}

	return tasks;
}

async Task PrintUrlsAsync(IEnumerable<string> urls)
{
	foreach (var url in urls)
	{
		await Task.Run(() => ShowDownloadedData(url));
	}
}

async Task PrintUrlsAsyncParallel(IEnumerable<string> urls)
{
	Parallel.ForEach(urls,async url =>
	{
		await Task.Run(() => ShowDownloadedData(url));
	});
	
}

async Task PrintUrlsAsyncParallelAsync(IEnumerable<string> urls)
{
	await Parallel.ForEachAsync(urls, async (url, token) =>
	{
		await Task.Run(() => ShowDownloadedData(url), token);
	});
}

async Task PrintUrlsAsyncParallelAsyncAsync(IEnumerable<string> urls)
{
	await Parallel.ForEachAsync(urls, async (url, token) =>
	{
		ShowDownloadedDataAsync(url);
	});
}

async Task PrintUrlsAsyncParallelWithHttpClientAsync(IEnumerable<string> urls)
{
	await Parallel.ForEachAsync(urls, async (url, token) =>
	{
		await ShowDownloadedDataWithHttpClientAsync(url);
	});
}

void ShowDownloadedData(object? url)
{
	var webClient = new WebClient();
	var pageContent = webClient.DownloadString((string)url);

	WriteLine($"{url} - {pageContent.Length}");
}

void ShowDownloadedDataAsync(object? url)
{
	var webClient = new WebClient();
	webClient.DownloadStringAsync( new Uri((string)url));

    webClient.DownloadStringCompleted += (sender, e) =>
    {
		WriteLine($"{url} - {e.Result.Length}");
	};	
}

async Task ShowDownloadedDataWithHttpClientAsync(string url)
{
	var pageContent = await client.GetStringAsync(url);
	WriteLine($"{url} - {pageContent.Length}");
}

class ThreadWithState
{
	private string _url;

	public ThreadWithState(string url)
    {
		_url = url;
    }

	public void ShowDownloadedData()
	{
		var webClient = new WebClient();
		var pageContent = webClient.DownloadString(_url);

		WriteLine($"{_url} - {pageContent.Length}");
	}
}