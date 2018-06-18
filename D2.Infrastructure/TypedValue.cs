﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace D2.Infrastructure
{
    public struct TypedValue : IEquatable<TypedValue>,
        IComparable<TypedValue>,
        IComparable
    {
        private decimal Value_;
        private string Unit_;

        public string Unit { get => Unit_; }
        public decimal Value { get => Value_; }

        public TypedValue(decimal value, string unit)
        {
            Value_ = value;
            Unit_ = unit;
        }

        public TypedValue(TypedValue value)
        {
            Value_ = value.Value_;
            Unit_ = value.Unit_;
        }

        public int CompareTo(TypedValue other)
        {
            if (other.Unit_ != Unit_) throw new Exception(string.Format("cannot compare '{0}' to '{1}'", other.Unit_, Unit_));
            return Value_.CompareTo(other.Value_);
        }

        public int CompareTo(object obj)
        {
            if (obj == null || obj.GetType() != typeof(TypedValue)) throw new ArgumentException("object is null or not of type TypedValue");
            return CompareTo((TypedValue)obj);
        }

        public bool Equals(TypedValue other)
        {
            return Unit_ == other.Unit_ && Value_ == other.Value_;
        }

        public static TypedValue operator -(TypedValue value)
        {
            return new TypedValue(-value.Value_, value.Unit_);
        }

        public static TypedValue operator +(TypedValue value)
        {
            return value;
        }

        static InvalidOperationException DifferentUnits()
        {
            return new InvalidOperationException("cannot calculate with differing units");
        }

        public static TypedValue operator +(TypedValue left, TypedValue right)
        {
            if (left.Unit_ != right.Unit)
            {
                throw DifferentUnits();
            }

            return new TypedValue(left.Value_ + right.Value_, left.Unit);
        }

        public static TypedValue operator -(TypedValue left, TypedValue right)
        {
            if (left.Unit_ != right.Unit)
            {
                throw DifferentUnits();
            }

            return new TypedValue(left.Value_ - right.Value_, left.Unit);
        }

        public static TypedValue operator *(TypedValue left, Decimal right)
        {
            return new TypedValue(left.Value_ * right, left.Unit);
        }

        public static TypedValue operator /(TypedValue left, Decimal right)
        {
            return new TypedValue(left.Value_ / right, left.Unit);
        }

        public static bool operator ==(TypedValue left, TypedValue right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TypedValue left, TypedValue right)
        {
            return !left.Equals(right);
        }

        public static bool operator >=(TypedValue left, TypedValue right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <=(TypedValue left, TypedValue right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(TypedValue left, TypedValue right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <(TypedValue left, TypedValue right)
        {
            return left.CompareTo(right) < 0;
        }

        public override int GetHashCode()
        {
            return Value_.GetHashCode() | (Unit_ != null ? Unit_.GetHashCode() : 0);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(TypedValue)) return false;
            if (((TypedValue)obj).Unit_ != Unit_) return false;
            return ((TypedValue)obj).Value_ == Value_;
        }


        public override string ToString()
        {
            return string.Format("{0} {1}", Value_, Unit_);
        }
    }
}