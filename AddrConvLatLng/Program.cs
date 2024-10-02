using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // AddrConv クラスのインスタンスを作成
        var converter = new AddrConv();

        // 東京の住所を指定
        string tokyoAddress = "東京";

        // 東京の緯度経度を取得
        var (lat, lng) = await converter.ConvLatLng(tokyoAddress);

        // 結果を表示
        if (lat != -1 && lng != -1)
        {
            Console.WriteLine($"Latitude: {lat}, Longitude: {lng}");
        }
        else
        {
            Console.WriteLine("Failed to retrieve the latitude and longitude.");
        }

        // リソースを解放
        converter.Close();
    }
}
