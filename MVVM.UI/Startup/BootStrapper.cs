﻿using Autofac;
using MVVM.DataAccess;
using MVVM.UI.Data;
using MVVM.UI.Data.Lookups;
using MVVM.UI.View.Services;
using MVVM.UI.ViewModel;
using Prism.Events;

namespace MVVM.UI.Startup
{
   public class BootStrapper
    {
        public IContainer BootStrap()
        {

            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<FriendDbContext>().AsSelf();
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FriendDetailViewModel>().As<IFriendDetailViewModel>();
            builder.RegisterType<FriendDataRepository>().As<IFriendDataRepository>();
            builder.RegisterType<LookUpDataService>().AsImplementedInterfaces();
            return builder.Build();
        }
    }
}
