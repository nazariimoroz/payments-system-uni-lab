﻿namespace payments_system_lib.Interfaces
{
    public interface IDbAgent
    {
        int Id { get; set; }
        bool SaveToDb();
    }
}
