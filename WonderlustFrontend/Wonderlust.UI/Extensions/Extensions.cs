using Wonderlust.UI.Pages;
using Wonderlust.UI.ViewModels;

namespace Wonderlust.UI.Extensions;

public static class UiExtensions
{
    public static IServiceCollection AddPages(this IServiceCollection services)
    {
        services.AddTransient<Posts>();
        services.AddTransient<Communities>();
        services.AddTransient<Comments>();
        services.AddTransient<CommunityFormPage>();
        services.AddTransient<PostFormPage>();

        return services;
    }

    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddTransient<PostsViewModel>();
        services.AddTransient<CommunitiesViewModel>();
        services.AddTransient<CommentPageViewModel>();
        services.AddTransient<CommentViewModel>();
        services.AddTransient<CommunityFormViewModel>();
        services.AddTransient<PostFormViewModel>();

        return services;
    }
}