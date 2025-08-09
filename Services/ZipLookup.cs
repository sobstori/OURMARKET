// Services/ZipLookup.cs
using CsvHelper;
using CsvHelper.Configuration;
using System.Formats.Asn1;
using System.Globalization;

namespace OurMarketBackend.Services
{
    public class ZipLookup : IZipLookup
    {
        private readonly Dictionary<string, (string City, string State)> _map;

        private sealed class ZipRow
        {
            public string? zip { get; set; }            // may be "00501" or 501 in source
            public string? primary_city { get; set; }
            public string? state { get; set; }
        }

        private sealed class ZipRowMap : ClassMap<ZipRow>
        {
            public ZipRowMap()
            {
                Map(m => m.zip).Name("zip");
                Map(m => m.primary_city).Name("primary_city");
                Map(m => m.state).Name("state");
            }
        }

        public ZipLookup(IWebHostEnvironment env)
        {
            var path = Path.Combine(env.ContentRootPath, "App_Data", "zip_code_database.csv");

            using var reader = new StreamReader(path);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLowerInvariant(),
                BadDataFound = null,
                MissingFieldFound = null,
                DetectColumnCountChanges = false
            };
            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<ZipRowMap>();

            _map = new(StringComparer.Ordinal);
            foreach (var row in csv.GetRecords<ZipRow>())
            {
                if (string.IsNullOrWhiteSpace(row.zip) ||
                    string.IsNullOrWhiteSpace(row.primary_city) ||
                    string.IsNullOrWhiteSpace(row.state)) continue;

                var z = row.zip.Trim();
                // Some files store zip as numeric; normalize to 5 digits
                if (int.TryParse(z, out var zi)) z = zi.ToString("D5");

                _map[z] = (row.primary_city.Trim(), row.state.Trim().ToUpperInvariant());
            }
        }

        public bool TryGet(string zip, out (string City, string State) info)
        {
            info = default; // fixes compiler error

            var key = zip?.Trim();
            if (int.TryParse(key, out var zi))
                key = zi.ToString("D5");

            return key is not null && _map.TryGetValue(key, out info);
        }

    }
}
