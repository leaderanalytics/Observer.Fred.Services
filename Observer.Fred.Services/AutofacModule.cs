namespace LeaderAnalytics.Observer.Fred.Services;

public class AutofacModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        // Autofac & AdaptiveClient

        // Don't forget to do this:
        builder.RegisterModule(new LeaderAnalytics.AdaptiveClient.EntityFrameworkCore.AutofacModule());

        RegistrationHelper registrationHelper = new RegistrationHelper(builder);
        registrationHelper.RegisterModule(new AdaptiveClientModule());
    }
}
