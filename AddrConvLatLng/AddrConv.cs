using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


internal class AddrConv
{
    private static IWebDriver webDriver  = null;

    public AddrConv() {
        webDriver  = new ChromeDriver();
    }
    ~AddrConv() {
        webDriver.Quit();
    }

    public void Open()
    {
        webDriver = new ChromeDriver();
    }
    public void Close() { 
        webDriver.Close();
    }

    internal async Task<(double lat, double lng)> ConvLatLng(string addr)
    {
        if (webDriver == null || !webDriver.WindowHandles.Any())
            webDriver = new ChromeDriver();

        // データ格納
        string url;
        double lat = -1, lng = -1;

        // タイムアウト用
        int cnt = 0;
        const int timeout = 100;

        webDriver.Navigate().GoToUrl("https://www.google.com/maps/place/" + addr);

        // 変更されるまで待機
        while (!(url = webDriver.Url.ToString()).Contains("@") && cnt < timeout)
        {
            // 1秒待機
            System.Threading.Thread.Sleep(1000);
            cnt++;
        }

        if (cnt == timeout)
            return (lat, lng);

        // 経度・緯度変換
        var parsedData = parseLatLng(url);
        if (parsedData == null)
            return (lat, lng);

        // 緯度・経度を格納
        lat = parsedData.Value.lat;
        lng = parsedData.Value.lng;

        return (lat, lng);
    }

    // URLから経度と緯度を取得する
    private static (double lat, double lng)? parseLatLng(string url)
    {
        Match match = Regex.Match(url, @"@(-?\d+\.\d+),(-?\d+\.\d+)");
        if (match.Success)
        {
            double lat = double.Parse(match.Groups[1].Value);
            double lng = double.Parse(match.Groups[2].Value);
            return (lat, lng);
        }
        return null;
    }

}