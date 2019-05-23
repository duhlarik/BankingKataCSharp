using System;
using System.Globalization;
using Newtonsoft.Json;

namespace BankingKataAPI.Models
{
    public sealed class Money : IEquatable<Money>, IComparable, IComparable<Money>
    {

        private static readonly int[] Cents = { 1, 10, 100, 1000 };

        private readonly CultureInfo _cultureInfo;

        private readonly RegionInfo _regionInfo;

        public long LongAmount { get; private set; }

        public Money() : this(0, CultureInfo.CurrentCulture) { }

        public Money(decimal amount) : this(amount, CultureInfo.CurrentCulture) { }

        public Money(long longAmount) : this(longAmount, CultureInfo.CurrentCulture) { }

        public Money(string cultureName) : this(new CultureInfo(cultureName)) { }

        public Money(decimal amount, string cultureName) : this(amount, new CultureInfo(cultureName))
        { }

        public Money(CultureInfo cultureInfo) : this(0, cultureInfo) { }

        public Money(decimal amount, CultureInfo cultureInfo)
        {
            _cultureInfo = cultureInfo ?? throw new ArgumentNullException(nameof(cultureInfo));

            _regionInfo = new RegionInfo(cultureInfo.LCID);

            LongAmount = Convert.ToInt64(Math.Round(amount * CentFactor));

        }

        [JsonConstructor]
        public Money(long longAmount, string isoCurrencySymbol)
            : this(longAmount, CultureInfo.GetCultureInfo(isoCurrencySymbol))
        {

        }

        public Money(long longAmount, CultureInfo cultureInfo)
        {
            _cultureInfo = cultureInfo ?? throw new ArgumentNullException(nameof(cultureInfo));

            _regionInfo = RegionInfo.CurrentRegion;

            LongAmount = longAmount * CentFactor;

        }

        private int CentFactor => Cents[_cultureInfo.NumberFormat.CurrencyDecimalDigits];

        public string EnglishCultureName => _cultureInfo.Name;

        public string ISOCurrencySymbol => _regionInfo.ISOCurrencySymbol;

        public decimal Amount => (decimal)LongAmount / CentFactor;

        public int DecimalDigits => _cultureInfo.NumberFormat.CurrencyDecimalDigits;

        public static bool operator >(Money first, Money second)
        {

            AssertSameCurrency(first, second);

            return first.LongAmount > second.LongAmount;

        }

        public static bool operator >=(Money first, Money second)
        {

            AssertSameCurrency(first, second);

            return first.LongAmount >= second.LongAmount;

        }

        public static bool operator <=(Money first, Money second)
        {

            AssertSameCurrency(first, second);

            return first.LongAmount <= second.LongAmount;

        }

        public static bool operator <(Money first, Money second)
        {

            AssertSameCurrency(first, second);

            return first.LongAmount < second.LongAmount;

        }

        public static Money operator +(Money first, Money second)
        {

            AssertSameCurrency(first, second);

            return new Money(first.Amount + second.Amount, first.EnglishCultureName);

        }

        public static Money operator -(Money first, Money second)
        {

            AssertSameCurrency(first, second);

            return new Money(first.Amount - second.Amount, first.EnglishCultureName);

        }

        public static Money Add(Money first, Money second)
        {

            return first + second;

        }


        public static implicit operator Money(decimal amount)

        {

            return new Money(amount, CultureInfo.CurrentCulture);

        }

        public static implicit operator Money(long amount)
        {

            return new Money(amount, CultureInfo.CurrentCulture);

        }

        public override bool Equals(object obj)
        {

            return (obj is Money) && Equals((Money)obj);

        }

        public override int GetHashCode()
        {

            return LongAmount.GetHashCode() ^ _cultureInfo.GetHashCode();

        }

        private static void AssertSameCurrency(Money first, Money second)
        {

            if (first.ISOCurrencySymbol != second.ISOCurrencySymbol)

                throw new InvalidOperationException("Money type mismatch.");

        }

        public bool Equals(Money other)
        {
            if (other is null) return false;

            return ISOCurrencySymbol == other.ISOCurrencySymbol 
                   && LongAmount == other.LongAmount;

        }

        public static bool operator ==(Money first, Money second)
        {

            if (object.ReferenceEquals(first, second)) return true;

            if (object.ReferenceEquals(first, null) || object.ReferenceEquals(second, null))

                return false;

            return (first.ISOCurrencySymbol == second.ISOCurrencySymbol &&

                    first.Amount == second.Amount);

        }

        public static bool operator !=(Money first, Money second)
        {

            return !first.Equals(second);

        }

        public static Money operator *(Money money, decimal value)
        {

            if (money == null) throw new ArgumentNullException("money");

            return new Money(Decimal.Floor(money.Amount * value), money.EnglishCultureName);

        }

        public static Money Multiply(Money money, decimal value)
        {

            return money * value;

        }

        public static Money operator /(Money money, decimal value)
        {

            if (money == null) throw new ArgumentNullException("money");

            return new Money(money.Amount / value, money.EnglishCultureName);

        }

        public static Money Divide(Money first, decimal value)
        {

            return first / value;

        }

        public Money Copy()
        {

            return new Money(Amount, _cultureInfo);

        }

        public Money Clone()
        {

            return new Money(_cultureInfo);

        }

        public int CompareTo(object obj)
        {

            if (obj == null)
            {

                return 1;

            }

            if (!(obj is Money))
            {

                throw new ArgumentException("Argument must be money");

            }

            return CompareTo((Money)obj);

        }

        public int CompareTo(Money other)
        {

            if (this < other)
            {

                return -1;

            }

            if (this > other)
            {

                return 1;

            }

            return 0;

        }

        public Money[] Allocate(long[] ratios)
        {

            return Allocate(ratios, CultureInfo.CurrentCulture);

        }

        //Foemmel’s Conundrum test from Martin Fowler’s

        //Patterns of Enterprise Application Architecture

        public Money[] Allocate(long[] ratios, CultureInfo cultureInfo)
        {

            if (ratios == null) throw new ArgumentNullException("ratios");

            long total = 0;

            for (int i = 0; i < ratios.Length; i++) total += ratios[i];

            long remainder = LongAmount;

            Money[] results = new Money[ratios.Length];

            for (int i = 0; i < results.Length; i++)
            {

                results[i] = new Money(Amount * ratios[i] / total, cultureInfo);

                remainder -= results[i].LongAmount;

                for (int j = 0; j < remainder; j++)
                {

                    results[i].LongAmount++;

                }

            }

            return results;

        }

        public override string ToString()
        {

            return Amount.ToString("C", _cultureInfo);

        }

        public string ToString(string format)
        {

            return Amount.ToString(format, this._cultureInfo);

        }

        public static Money LocalCurrency
        {

            get { return new Money(CultureInfo.CurrentCulture); }

        }

    }
}
