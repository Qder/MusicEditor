﻿using NHibernate;

namespace DevBridge.Templates.WebProject.DataContracts
{
    public interface ISessionFactoryProvider
    {
        ISessionFactory SessionFactory { get; }

        ISession Open();
    }
}