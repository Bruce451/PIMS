using AutoMapper;

namespace Pims.Api.Profiles.Converters
{
    public class ParcelIdConverter : IValueConverter<string, int>
    {
        public int Convert(string sourceMember, ResolutionContext context)
        {
            return int.TryParse(sourceMember.Replace("-", ""), out int pid) ? pid : 0;
        }
    }
}
