using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace CultureImportExport
{
    public class Program
    {
        private static string _culturePath;

        private static void Main(string[] args)
        {
            _culturePath = @"C:\Temp\Cultures\";

            try
            {
                Console.WriteLine("1. Get ru-UA culture and write to file");
                Console.WriteLine("2. Load culture from file and register");
                Console.WriteLine("3. Get ru-UA and show info");
                Console.WriteLine("4. Test that the culture works");
                Console.WriteLine("5. Unregister ru-UA");
                Console.WriteLine();
                var pressedKey = Console.ReadLine();

                switch (Convert.ToInt32(pressedKey))
                {
                    case 1:
                        GetCulture("ru-UA", true);
                        break;
                    case 2:
                        LoadCultureAndRegister("ru-UA");
                        break;
                    case 3:
                        GetCulture("ru-UA", false);
                        break;
                    case 4:
                        TestCulture();
                        break;
                    case 5:
                        UnregisterCulture();
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Press key to end program");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void GetCulture(string cultureName, bool save)
        {
            try
            {
                // Create a CultureAndRegionInfoBuilder object
                var culture = new CultureAndRegionInfoBuilder(
                    cultureName, CultureAndRegionModifiers.None);

                // Populate the new CultureAndRegionInfoBuilder object with culture information.
                culture.LoadDataFromCultureInfo(new CultureInfo(cultureName));

                // Populate the new CultureAndRegionInfoBuilder object with region information.
                culture.LoadDataFromRegionInfo(new RegionInfo(cultureName.Split(Convert.ToChar("-"))[1]));

                // Display some of the properties of the CultureAndRegionInfoBuilder object.
                Console.WriteLine("CultureName:. . . . . . . . . . {0}", culture.CultureName);
                Console.WriteLine("CultureEnglishName: . . . . . . {0}", culture.CultureEnglishName);
                Console.WriteLine("CultureNativeName:. . . . . . . {0}", culture.CultureNativeName);
                Console.WriteLine("GeoId:. . . . . . . . . . . . . {0}", culture.GeoId);
                Console.WriteLine("IsMetric: . . . . . . . . . . . {0}", culture.IsMetric);
                Console.WriteLine("ISOCurrencySymbol:. . . . . . . {0}", culture.ISOCurrencySymbol);
                Console.WriteLine("RegionEnglishName:. . . . . . . {0}", culture.RegionEnglishName);
                Console.WriteLine("RegionName: . . . . . . . . . . {0}", culture.RegionName);
                Console.WriteLine("RegionNativeName: . . . . . . . {0}", culture.RegionNativeName);
                Console.WriteLine("ThreeLetterISOLanguageName: . . {0}", culture.ThreeLetterISOLanguageName);
                Console.WriteLine("ThreeLetterISORegionName: . . . {0}", culture.ThreeLetterISORegionName);
                Console.WriteLine("ThreeLetterWindowsLanguageName: {0}", culture.ThreeLetterWindowsLanguageName);
                Console.WriteLine("ThreeLetterWindowsRegionName: . {0}", culture.ThreeLetterWindowsRegionName);
                Console.WriteLine("TwoLetterISOLanguageName: . . . {0}", culture.TwoLetterISOLanguageName);
                Console.WriteLine("TwoLetterISORegionName: . . . . {0}", culture.TwoLetterISORegionName);
                Console.WriteLine();

                // Save the culture to a file
                if (save)
                {
                    var pathToFile = Path.Combine(_culturePath, cultureName + ".culture");
                    Directory.CreateDirectory(_culturePath);
                    if (File.Exists(pathToFile))
                    {
                        File.Delete(pathToFile);
                    }
                    culture.Save(pathToFile);
                    Console.WriteLine(cultureName + " saved to " + pathToFile);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void LoadCultureAndRegister(string cultureName)
        {
            try
            {
                // Build and register a temporary culture with the name of what we want to import.
                // CreateFromLdml method will fail when trying to load a culture from file if it doesn't already exist.
                var tempCulture = new CultureAndRegionInfoBuilder(cultureName, CultureAndRegionModifiers.None);
                tempCulture.LoadDataFromCultureInfo(CultureInfo.CreateSpecificCulture("en-US"));
                tempCulture.LoadDataFromRegionInfo(new RegionInfo("en-US"));
                tempCulture.Register();
                Console.WriteLine($"Registered temporary culture for ({cultureName})");

                // Now load up the culture we actually want to import
                Console.WriteLine($"Loading culture from file ({cultureName})...");
                var culture = CultureAndRegionInfoBuilder.CreateFromLdml(Path.Combine(_culturePath, cultureName + ".culture"));

                // Unregister the temporary culture
                CultureAndRegionInfoBuilder.Unregister(cultureName);

                // Register the real culture loaded from file
                culture.Register();
                Console.WriteLine(culture.CultureName + " is now registered");
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        public static void TestCulture()
        {
            try
            {
                Console.WriteLine("Testing culture...");
                var c = new CultureInfo("ru-UA");
                Console.WriteLine("Culture: " + c.Name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        public static void UnregisterCulture()
        {
            Console.WriteLine("Removing ru-UA culture");
            CultureAndRegionInfoBuilder.Unregister("ru-UA");
            Console.WriteLine("ru-UA culture removed");
        }
    }
}
