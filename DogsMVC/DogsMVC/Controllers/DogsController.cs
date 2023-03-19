using DogsMVC.Models;
using DogsMVC.Viewmodels.Dogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

public class DogsController : Controller
{
    private readonly string _baseUrl;
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions _options;

    public DogsController(IConfiguration config, IHttpClientFactory clientFactory)
    {
        _baseUrl = config.GetSection("apiSettings:baseUrl").Value;
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };
    }

    public async Task<IActionResult> Index()
    {
        using var client = _clientFactory.CreateClient(); 
        var response = await client.GetAsync($"{_baseUrl}/dogs");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var json = await response.Content.ReadAsStringAsync();

        var dogs = JsonSerializer.Deserialize<List<DogViewModel>>(json, _options);

        return View(dogs);
    }

    public async Task<IActionResult> Details(int id)
    {
        using var client = _clientFactory.CreateClient(); 
        var response = await client.GetAsync($"{_baseUrl}/dogs/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var json = await response.Content.ReadAsStringAsync();

        var dogs = JsonSerializer.Deserialize<DogViewModel>(json, _options);

        return View(dogs);
    }

    public async Task<IActionResult> Delete(int Id)
    {
        using var client = _clientFactory.CreateClient(); 
        var response = await client.DeleteAsync($"{_baseUrl}/dogs/{Id}");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        using var client = _clientFactory.CreateClient(); 
        var response = await client.GetAsync($"{_baseUrl}/dogs/{Id}");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var json = await response.Content.ReadAsStringAsync();

        var dogs = JsonSerializer.Deserialize<DogViewModel>(json, _options);

        return View(dogs);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(DogViewModel model)
    {
        using var client = _clientFactory.CreateClient(); 

        var json = JsonSerializer.Serialize(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"{_baseUrl}/dogs/{model.Id}", content);

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }


        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Create()
    {
        var dog = new DogViewModel();

        return View(dog); 
    }

    [HttpPost]
    public async Task<IActionResult> Create(DogModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        using var client = _clientFactory.CreateClient(); 
        var json = JsonSerializer.Serialize(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{_baseUrl}/dogs", content);

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}
