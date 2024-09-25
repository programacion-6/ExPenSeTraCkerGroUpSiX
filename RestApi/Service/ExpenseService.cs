namespace RestApi.Service;

using System;
using System.Collections.Generic;
using RestApi.Domain;
using RestApi.Persistence;

public class ExpenseService(ApplicationDbContext context) : BaseService<Expense>
{
    private readonly ApplicationDbContext _Context = context;

    public bool Create(Expense item)
    {
        GetList().Add(item);
        return GetItem(item.Id) != null;
    }

    public bool Delete(Guid id)
    {
        return GetList().Remove(id);
    }

    public List<Expense> GetList()
    {
        Guid guid= Guid.NewGuid();
        List<Expense> expenses = _Context.UsersItems.Find(guid).Expenses;
        if (expenses == null)
        {

        } 
        return expenses;
    }

    public Expense GetItem(Guid id)
    {
        Expense expense = GetList().Find(item => item.Id == id);
        if (expense == null)
        {

        }
        return expense;
    }

    public bool Update(Guid Id, Expense item)
    {
        throw new NotImplementedException();
    }
}