using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var sourceType = typeof(TSource);

            var sourceMembers = GetMembers(sourceType);
            var targetMembers = GetMembers(typeof(TDestination));

            var paramExpr = Expression.Parameter(typeof(object));
            var sourceExpr = Expression.Convert(paramExpr, sourceType);

            var bindings = new List<MemberBinding>();

            foreach (var targetFieldy in targetMembers)
            {
                if (sourceMembers.TryGetValue(targetFieldy.Key, out var sourceField))
                {
                    bindings.Add(Expression.Bind(targetFieldy.Value, Expression.PropertyOrField(sourceExpr, sourceField.Name)));
                }
            }

            var resultExpr = Expression.MemberInit(Expression.New(typeof(TDestination)), bindings);
            var mapperExpr = Expression.Lambda<Func<TSource, TDestination>>(resultExpr, paramExpr);

            return new Mapper<TSource, TDestination>(mapperExpr.Compile());
        }

        private Dictionary<string, MemberInfo> GetMembers(Type objType)
        {
            var membersDictionary = new Dictionary<string, MemberInfo>();
            var fields = objType.GetFields();
            var properties = objType.GetProperties();

            foreach (var memberInfo in fields)
            {
                membersDictionary.Add(memberInfo.Name, memberInfo);
            }

            foreach (var memberInfo in properties)
            {
                membersDictionary.Add(memberInfo.Name, memberInfo);
            }

            return membersDictionary;
        }
    }
}
