using MudBlazor;

namespace ProjectFlow.Web;

public class Configuration
{
    #region Constants

    private const string PrincipalColor = "#4E61FF";
    public const string HttpClientName = "ProjectFlow";
    public const string TokenName = "access_token";
    #endregion

    #region Static Properties

    public static string BackendUrl { get; set; } = string.Empty;

    public static readonly string SrcLogo = "/src/images/logo.png";

    public static readonly MudTheme Theme = new()
    {
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Poppins", "sans-serif"], 
                FontSize = "14",
            }
        },
        PaletteLight = new PaletteLight
        {
            Primary = PrincipalColor,
            //PrimaryContrastText = new MudColor("#000000"),
            //Secondary = Colors.LightBlue.Darken3,
            //Background = Colors.Gray.Lighten4,
            //AppbarBackground = PrincipalColor,
            //AppbarText = Colors.Shades.White,
            //TextPrimary = Colors.Shades.Black,
            //DrawerText = Colors.Shades.White,
            //DrawerBackground = PrincipalColor
        },
        //PaletteDark = new PaletteDark
        //{
        //    Primary = Colors.LightBlue.Accent3,
        //    Secondary = Colors.LightBlue.Darken3,
        //    AppbarBackground = PrincipalColor,
        //    AppbarText = Colors.Shades.White,
        //    PrimaryContrastText = new MudColor("#000000"),
        //    DrawerBackground = PrincipalColor,
        //    DrawerText = Colors.Shades.White,
        //    TextPrimary = Colors.Shades.White
        //}
    };

    #endregion
}