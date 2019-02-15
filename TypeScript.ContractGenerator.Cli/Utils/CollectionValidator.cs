using System;

namespace SkbKontur.TypeScript.ContractGenerator.Cli.Utils
{
    internal class CollectionValidator<T> where T : class
    {
        public CollectionValidator(T[] items)
        {
            this.items = items;
        }

        public CollectionValidator<T> WithNoItemsError(string error)
        {
            noItemsError = error;
            return this;
        }

        public CollectionValidator<T> WithManyItemsError(Func<T[], string> errorFunc)
        {
            manyItemsError = errorFunc(items);
            return this;
        }

        public CollectionValidator<T> WithManyItemsError(string error)
        {
            manyItemsError = error;
            return this;
        }

        public (T value, string error) Single()
        {
            if (items.Length == 0)
            {
                return (null, noItemsError);
            }
            if (items.Length > 1)
            {
                return (null, manyItemsError);
            }
            return (items[0], null);
        }

        private readonly T[] items;
        private string noItemsError;
        private string manyItemsError;
    }
}