﻿// Copyright 2011 Chris Patterson
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace OdoyuleRules.Visualization
{
    using System;
    using System.Text;
    using RuntimeModel;
    using RuntimeModel.Nodes;


    public class TextRuntimeVisitor :
        RuntimeVisitorBase
    {
        const int PaddingWidth = 2;
        readonly StringBuilder _sb;
        int _depth;
        string _padding;

        public TextRuntimeVisitor()
        {
            _sb = new StringBuilder();

            _depth = 0;
            _padding = "";
        }

        public override bool Visit(RulesEngine rulesEngine, Func<RuntimeVisitor, bool> next)
        {
            Append("Rules Engine");

            return Indent(next);
        }

        public override bool Visit<T>(JoinNode<T> node, Func<RuntimeVisitor, bool> next)
        {
            Append("JoinNode[{0}]", Tokens<T>());

            return Indent(next);
        }

        public override bool Visit<T, TOutput>(LeftJoinNode<T, TOutput> node, Func<RuntimeVisitor, bool> next)
        {
            Append("LeftJoinNode[{0}] => {1}", Tokens<T>(), Tokens<TOutput>());

            return Indent(next);
        }

        public override bool Visit<TLeft, TRight>(OuterJoinNode<TLeft, TRight> node, Func<RuntimeVisitor, bool> next)
        {
            Append("OuterJoinNode[{0},{1}] => {2}", Tokens<TLeft>(), Tokens<TRight>(), Tokens<Tuple<TLeft,TRight>>());

            return Indent(next);
        }

        public override bool Visit<T>(AlphaNode<T> node, Func<RuntimeVisitor, bool> next)
        {
            Append("AlphaNode[{0}]", Tokens<T>());

            return Indent(next);
        }

        public override bool Visit<TInput, TOutput>(WidenTypeNode<TInput, TOutput> node,
                                                    Func<RuntimeVisitor, bool> next)
        {
            Append("ConvertNode[{0}] => {1}", Tokens<TInput>(), Tokens<TOutput>());

            return Indent(next);
        }

        public override bool Visit<T>(DelegateProductionNode<T> node, Func<RuntimeVisitor, bool> next)
        {
            Append("DelegateProductionNode[{0}]", Tokens<T>());

            return Indent(next);
        }

        public override bool Visit<T, TFact>(AddFactNode<T, TFact> node, Func<RuntimeVisitor, bool> next)
        {
            Append("AddFactProductionNode[{0}] => {1}", Tokens<T>(), typeof (TFact));

            return base.Visit(node, next);
        }

        public override bool Visit<T, TProperty, TValue>(PropertyNode<T, TProperty, TValue> node, Func<RuntimeVisitor, bool> next)
        {
            Append("PropertyNode[{0}].{1} ({2})", Tokens<T>(), node.PropertyInfo.Name, typeof (TProperty).GetShortName());

            return Indent(next);
        }

        public override bool Visit<T>(ConstantNode<T> node, Func<RuntimeVisitor, bool> next)
        {
            Append("v[{0}]", Tokens<T>());

            return Indent(next);
        }

        public override bool Visit<T>(ConditionNode<T> node, Func<RuntimeVisitor, bool> next)
        {
            Append("ConditionNode[{0}]", Tokens<T>());

            return Indent(next);
        }

        public override bool Visit<T, TProperty>(EqualNode<T, TProperty> node,
                                                 Func<RuntimeVisitor, bool> next)
        {
            Append("EqualNode[{0}] ({1})", Tokens<T>(), typeof (TProperty).GetShortName());

            return Indent(next);
        }

        public override bool Visit<T, TProperty>(ValueNode<T, TProperty> node, Func<RuntimeVisitor, bool> next)
        {
            Append("ValueNode[{0}] == {1}", Tokens<T>(), node.Value);

            return Indent(next);
        }

        public override bool Visit<T, TProperty>(CompareNode<T, TProperty> node, Func<RuntimeVisitor, bool> next)
        {
            Append("CompareNode[{0},{1}] {2} {3}", Tokens<T>(), typeof (TProperty).GetShortName(), node.Comparator.ToString(),
                   node.Value.ToString());

            return Indent(next);
        }

        public override bool Visit<T, TProperty>(NotNullNode<T, TProperty> node, Func<RuntimeVisitor, bool> next)
        {
            Append("NotNullNode[{0}] != null", Tokens<T>());

            return Indent(next);
        }

        public override bool Visit<T, TProperty>(ExistsNode<T, TProperty> node, Func<RuntimeVisitor, bool> next)
        {
            Append("ExistsNode[{0}]", Tokens<T>());

            return Indent(next);
        }

        public override bool Visit<T, TProperty, TElement>(EachNode<T, TProperty, TElement> node,
                                                           Func<RuntimeVisitor, bool> next)
        {
            Append("EachNode[{0}]", Tokens<T>());

            return Indent(next);
        }

        void Append(string format, params object[] args)
        {
            Append(string.Format(format, args));
        }

        void Append(string text)
        {
            _sb.Append(_padding).AppendLine(text);
        }

        string Tokens<T>()
        {
            return typeof (T).Tokens();
        }

        bool Indent(Func<RuntimeVisitor, bool> next)
        {
            _depth++;
            string previous = _padding;
            _padding = new string(' ', _depth*PaddingWidth);

            bool result = next(this);

            _depth--;
            _padding = previous;

            return result;
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}