using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

class Program
{
    static void Main()
    {
        try
        {
            // Specify the correct path to the JSON file
            string jsonFilePath = "billing.json";
            
            // Check if the file exists
            if (!File.Exists(jsonFilePath))
            {
                Console.WriteLine($"Error: The file '{jsonFilePath}' was not found. Please ensure the file is in the correct location.");
                return;
            }

            // Load the billing data from the JSON file
            string json = File.ReadAllText(jsonFilePath);

            // Deserialize the JSON data into a list of Billing objects
            List<Billing>? billings = JsonSerializer.Deserialize<List<Billing>>(json);

            // Check if the deserialization was successful
            if (billings is null || !billings.Any())
            {
                Console.WriteLine("No data found in the JSON file.");
                return;
            }

            // Filter out the days with zero billing (ignore days without revenue)
            var daysWithBilling = billings.Where(b => b.Value > 0).ToList();

            // Ensure there are days with billing to calculate
            if (daysWithBilling.Count == 0)
            {
                Console.WriteLine("No billing data to process.");
                return;
            }

            // Calculate the minimum billing value
            double minBilling = daysWithBilling.Min(b => b.Value);

            // Calculate the maximum billing value
            double maxBilling = daysWithBilling.Max(b => b.Value);

            // Calculate the monthly average considering only the days with billing
            double monthlyAverage = daysWithBilling.Average(b => b.Value);

            // Count the number of days with billing above the monthly average
            int daysAboveAverage = daysWithBilling.Count(b => b.Value > monthlyAverage);

            // Display the results
            Console.WriteLine($"Minimum billing: {minBilling:C}");
            Console.WriteLine($"Maximum billing: {maxBilling:C}");
            Console.WriteLine($"Days with billing above the monthly average: {daysAboveAverage}");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Error: The file 'billing.json' was not found. Please ensure the file is in the correct location.");
        }
        catch (JsonException)
        {
            Console.WriteLine("Error: The JSON data could not be parsed. Please check the file format.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
}

// Class representing a billing entry with day and value
class Billing
{
    public int Day { get; set; }
    public double Value { get; set; }
}
