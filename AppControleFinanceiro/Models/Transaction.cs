﻿using LiteDB;

namespace AppControleFinanceiro.Models
{
    public class Transaction
    {
        [BsonId]
        public int Id { get; set; }
        public TransactionType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset Date { get; set; }
        public double Value { get; set; }
    }

    public enum TransactionType
    {
        Income,
        Expense
    }
}
