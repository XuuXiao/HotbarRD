﻿using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace HotbarRD;

// This class can be overhauled to include any type of file, but mostly because it's not necessary,
// I'm making it only compatible with PNGs. JPG and BMP compatibility can be made very easily.
internal class AssetsManager
{
    internal static AssetsManager? Singleton { get; private set; }
    private readonly Dictionary<string, Image> Assets;

    internal AssetsManager(string assetBundlePath)
    {
        Singleton = this;
        Assets = [];
        LoadAllAssets();
    }

    internal void LoadAllAssets()
    {
        var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
        foreach (var name in names)
        {
            if (!name.EndsWith(".png"))
                continue;
            var res = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(name));
            Assets.Add(name.Split('.')[^1], res);
        }
    }

    internal bool TryGetAsset(string name, out Image res)
    {
        return Assets.TryGetValue(name, out res);
    }

    internal Image[] SearchAssets(string searchArgument)
    {
        var regex = new Regex($"${searchArgument}", RegexOptions.IgnoreCase);
        List<Image> Results = [];
        foreach(var asset in Assets)
        {
            if (!regex.IsMatch(asset.Key))
                continue;

            Results.Add(asset.Value);
        }
        return Results.ToArray();
    }

    internal Image[] SearchAssets(CustomFrames frameType)
    {
        return SearchAssets(frameType.ToString());
    }
}