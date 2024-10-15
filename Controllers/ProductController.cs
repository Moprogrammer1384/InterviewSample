using InterviewSample.Application.Contract;
using InterviewSample.Application.Dtos;
using InterviewSample.Context;
using InterviewSample.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InterviewSample.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly Random random;
    private readonly IUrlRepository urlRepository;

    public ProductController(IUrlRepository urlRepository)
    {
        this.random = new Random();
        this.urlRepository = urlRepository;
    }

    [HttpGet("Track/")]
    public IActionResult TrackingCode()
    {
        int trackingCode = random.Next(0, 4000);
        return Ok(trackingCode);
    }

    [HttpGet("{trackingCode}")]
    public IActionResult Generate([FromRoute]int trackingCode, 
                                  [FromServices]LinkGenerator linkGenerator)
    {
        string link = $"https://localhost:7016/Product/{trackingCode}";
        
        var url = urlRepository.Get(link);
        if (url is not null)
        {
            url.Views++;
            urlRepository.Update(url);
            return Ok(new UrlDto(url.Url, url.Views));
        }

        urlRepository.Update(new Models.UrlInfo
        {
            Url = link,
            Views = 1
        });

        return Ok(new UrlDto(link, 1));
    }
         
}
