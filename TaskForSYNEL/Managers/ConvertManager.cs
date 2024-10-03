using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace TaskForSYNEL.Managers;

public class ConvertManager: DefaultTypeConverter
{
    private readonly string[] _dateFormats = new[] { "dd/MM/yyyy", "MM/dd/yyyy", "d/M/yyyy" };

    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (DateTime.TryParseExact(text, _dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            return date;
        }
        throw new TypeConverterException(this, memberMapData, text, row.Context);
    }
}