# DangEasy-BlobStorage-Azure

A simple intuitive library for Azure Blob Storage.

# Installation

Use NuGet to install the [package](https://www.nuget.org/packages/DangEasy.BlobStorage.Azure/).

```
PM> Install-Package DangEasy-BlobStorage-Azure
```


## Examples
### Setup 
```
 _client = new BlobStorageClient("<ConnectionString>", "<ContainerName>");
```

### Usage
```
// upload file
var filePath = $"foldername/example.txt"; // relative to the container
var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
System.Console.WriteLine($"\nUploading file {filePath}");
var saved = _client.SaveAsync(filePath, stream).GetAwaiter().GetResult();
System.Console.WriteLine($"\nUploaded: {saved}");


// file exists
System.Console.WriteLine($"\nFile exists?");
var exists = _client.ExistsAsync(filePath).Result;
System.Console.WriteLine($"{exists}");


// file info
System.Console.WriteLine($"\nBlob info:");
var info = _client.GetInfoAsync(filePath).Result;
System.Console.WriteLine($"{info.Path}, Created:{info.Created}, Modified:{info.Modified}, Size:{info.Size}");


// show root blobs - should have 1 folder
System.Console.WriteLine($"\nShowing blobs root");
var blobNames = _client.GetListAsync($"").Result;
blobNames.ToList().ForEach(x => System.Console.WriteLine(x));


// show files in folder - should have 1 file
System.Console.WriteLine($"\nShowing blobs in folder");
blobNames = _client.GetListAsync($"foldername").Result;
blobNames.ToList().ForEach(x => System.Console.WriteLine(x));


// download file
System.Console.WriteLine($"\nDownloading {filePath}");
var downloadedStream = _client.GetAsync(filePath).Result as MemoryStream;
var resultString = Encoding.UTF8.GetString(downloadedStream.ToArray());
System.Console.WriteLine(resultString);


// delete file
System.Console.WriteLine($"\nDelete {filePath}");
var deleted = _client.DeleteAsync(filePath).Result;
System.Console.WriteLine($"Deleted: {deleted}");

System.Console.ReadLine();

```

