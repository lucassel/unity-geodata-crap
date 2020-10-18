using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// LDEM Data is height from LRO.
/// </summary>
[System.Serializable]
public class LDEMData
{
  public string DatasetID;

  /// <summary>
  /// Filename
  /// </summary>
  public string ProductID;

  /// <summary>
  /// Number of pixels per degree of longitude or latitude.
  /// </summary>
  public int MapResolution;

  /// <summary>
  /// X dimension
  /// </summary>
  public int ColumnCount;

  /// <summary>
  /// Y dimension
  /// </summary>
  public int RowCount;

  public float MinLat;
  public float MaxLat;
  public float MinLon;
  public float MaxLon;
  public float DataScale;

  /// <summary>
  /// Radius of the moon in kilometer.
  /// </summary>
  public float MoonRadius;

  /// <summary>
  /// IEEE754 float bit depth
  /// </summary>
  public int SampleBits;

  public string SampleType;

  // Number of bytes to represent a number in the .IMG file.
  public int DataSize;

  public float MinHeight, MaxHeight, TrueMin, TrueMax;

  public LDEMData(TextAsset labels)
  {
    var result = Regex.Split(labels.text, "\r\n|\r|\n").ToList();

    DatasetID = ReturnValueForToken("DATA_SET_ID", result);
    ProductID = ReturnValueForToken("PRODUCT_ID     ", result);
    MapResolution = int.Parse(ReturnValueForToken("MAP_RESOLUTION     ", result), CultureInfo.InvariantCulture);
    RowCount = int.Parse(ReturnValueForToken("LINE_LAST_PIXEL     ", result), CultureInfo.InvariantCulture);
    ColumnCount = int.Parse(ReturnValueForToken("SAMPLE_LAST_PIXEL     ", result), CultureInfo.InvariantCulture);

    MinLat = float.Parse(ReturnValueForToken("MINIMUM_LATITUDE     ", result), CultureInfo.InvariantCulture);
    MaxLat = float.Parse(ReturnValueForToken("MAXIMUM_LATITUDE     ", result), CultureInfo.InvariantCulture);
    MinLon = float.Parse(ReturnValueForToken("WESTERNMOST_LONGITUDE     ", result), CultureInfo.InvariantCulture);
    MaxLon = float.Parse(ReturnValueForToken("EASTERNMOST_LONGITUDE     ", result), CultureInfo.InvariantCulture);
    DataScale = float.Parse(ReturnValueForToken("SCALING_FACTOR     ", result), CultureInfo.InvariantCulture);
    MoonRadius = float.Parse(ReturnValueForToken("  OFFSET     ", result), CultureInfo.InvariantCulture);
    SampleBits = int.Parse(ReturnValueForToken("SAMPLE_BITS     ", result), CultureInfo.InvariantCulture);
    SampleType = ReturnValueForToken("SAMPLE_TYPE     ", result);

    if (SampleBits == 16)
    {
      DataSize = 2;
      // Integer .LBL files do not have MIN/MAX tokens but we'll go ahead and guess.
      MinHeight = -8.746f;
      MaxHeight = 10.380f;
      TrueMin = -100f;
      TrueMax = 100f;
    }
    else
    {
      DataSize = 4;
      MinHeight = float.Parse(ReturnValueForToken("  MINIMUM     ", result), CultureInfo.InvariantCulture);
      MaxHeight = float.Parse(ReturnValueForToken("  MAXIMUM     ", result), CultureInfo.InvariantCulture);
      TrueMin = MoonConstants.LowestPointOnTheMoon;
      TrueMax = MoonConstants.HighestPointOnTheMoon;
    }
  }

  private string ReturnValueForToken(string token, List<string> lines)
  {
    var result = string.Empty;
    foreach (var line in lines)
    {
      // Look for token.
      if (line.Contains(token))
      {
        // Look for = symbol
        if (line.Contains("="))
        {
          var split = line.Split('=');
          // Look for trailing junk to remove.
          if (split[1].Contains('<'))
          {
            var final = split[1].Split('<');
            return final[0];
          }

          return split[1];
        }
      }
    }

    return result;
  }
}