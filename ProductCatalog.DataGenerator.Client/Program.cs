using Microsoft.Extensions.Configuration;
using ProductCatalog.DataGenerator.Client;
using System.Net.Http.Headers;
using System.Net.Mime;


var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var hostSection = config.GetRequiredSection("host");
var folderPathSection = config.GetRequiredSection("folderPath");
var productsAmountSection = config.GetRequiredSection("productsAmount");

if(hostSection.Exists() == false || folderPathSection.Exists() == false ||
    productsAmountSection.Exists() == false)
{
    Console.WriteLine("Some of required section has't value or does't exist.");
    Console.ReadLine();
    return;
}

string folderPath = folderPathSection.Value;
string host = hostSection.Value;
string amount = productsAmountSection.Value;

var random = new Random();

if (int.TryParse(amount, out int productsAmount) == false)
{
    Console.WriteLine($"Can't parse to int value: {amount}");
    return;
}

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri(host);

DirectoryInfo rootFolder = new DirectoryInfo(folderPath);

var folders = rootFolder.GetDirectories();

var foldersCount = folders.Length;

List<Product> products = new List<Product>();

for (int i = 0; i < productsAmount; i++)
{
    int randomDirectoryIndex = random.Next(0, foldersCount - 1);

    DirectoryInfo randomFolder = folders.ElementAt(randomDirectoryIndex);

    var filesPath = GetRandomFilesPathFromFolder(randomFolder, random);

    var product = new Product()
    {
        Title = $"{i}_{randomFolder.Name}_{Guid.NewGuid().ToString()}",
        Description = "generated"
    };

    await SendProduct(product, filesPath, httpClient);

}

Console.ReadLine();


static IEnumerable<string> GetRandomFilesPathFromFolder(DirectoryInfo folder, Random random)
{
    List<string> pathes = new List<string>();

    var files = folder.GetFiles();

    int count = random.Next(0, 5);

    for (int i = 0; i < count; i++)
    {
        int randomFileIndex = random.Next(0, files.Length - 1);

        FileInfo fileInfo = files.ElementAt(randomFileIndex);

        Console.WriteLine($"random file: [{fileInfo.Name}] from folder [{folder.Name}");

        pathes.Add(fileInfo.FullName);
    }


    return pathes;
}

static async Task SendProduct(Product product, IEnumerable<string> files,
    HttpClient httpClient)
{
    using var formData = new MultipartFormDataContent();

    formData.Add(new StringContent(product.Title), "title");
    formData.Add(new StringContent(product.Description), "description");

    foreach (var filePath in files)
    {
        var fileName = Path.GetFileName(filePath);

        var bytes = await File.ReadAllBytesAsync(filePath);

        var fileContent = new ByteArrayContent(bytes);

        fileContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Image.Jpeg.ToString());

        formData.Add(fileContent, "files", fileName);
    }

    var response = await httpClient.PostAsync("api/products", formData);

    if(response.IsSuccessStatusCode == false)
    {
        throw new Exception($"Что-то пошло не так. Отправка не завершилась успешно.");
    }

    Console.WriteLine(await response.Content.ReadAsStreamAsync());
}











