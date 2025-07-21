using System.Runtime.CompilerServices;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;
using UnrealSharp.UnrealSharpCore;

namespace UnrealInject;

internal class UnrealSubsystemSource<T>(FSubsystemCollectionBaseRef collection) : IRegistrationSource
    where T : USubsystem {
  public bool IsAdapterForIndividualComponents => false;
  private readonly IntPtr _container = Unsafe.BitCast<FSubsystemCollectionBaseRef, IntPtr>(collection);

  public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor) {
    if (service is not TypedService typedService)
      return [];

    var serviceType = typedService.ServiceType;

    // Check if the requested type is a subsystem
    if (!serviceType.IsSubclassOf(typeof(T)))
      return [];

    // Create registration-builder
    var rb = RegistrationBuilder
      .ForDelegate<USubsystem>((_, _) => {
        var collection = Unsafe.BitCast<IntPtr, FSubsystemCollectionBaseRef>(_container);
        return collection.InitializeRequiredSubsystem(new TSubclassOf<T>(serviceType));
      })
      .As(serviceType)
      .SingleInstance();

    return [rb.CreateRegistration()];
  }
}
