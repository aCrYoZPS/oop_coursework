using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Wonderlust.UI.Application.Services.Comments;
using Wonderlust.UI.Application.Services.Communities;
using Wonderlust.UI.Application.Services.Posts;
using Wonderlust.UI.Application.SessionManager;
using Wonderlust.UI.Extensions;

namespace Wonderlust.UI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services
            .AddPages()
            .AddViewModels()
            .AddSingleton<SessionManager>();

        builder.Services.AddHttpClient<IPostService, PostService>(opt =>
            opt.BaseAddress = new Uri("http://localhost:5097/posts")
        );
        builder.Services.AddHttpClient<ICommunityService, CommunityService>(opt =>
            opt.BaseAddress = new Uri("http://localhost:5097/communities")
        );
        builder.Services.AddHttpClient<ICommentService, CommentService>(opt =>
            opt.BaseAddress = new Uri("http://localhost:5097/comments")
        );


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}