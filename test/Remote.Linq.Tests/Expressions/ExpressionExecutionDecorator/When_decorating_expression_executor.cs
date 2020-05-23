﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Remote.Linq.Tests.Expressions.ExpressionExecutionDecorator
{
    using Aqua.Dynamic;
    using Remote.Linq.Expressions;
    using Shouldly;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class When_decorating_expression_executor
    {
        [Fact]
        public void Should_apply_all_custom_strategies_and_return_expected_result()
        {
            var decorator = new TestExpressionExecutionDecorator(new DefaultExpressionExecutor(null));

            decorator
                .Execute(TestExpressionExecutionDecorator.Step0_Expression)
                .ShouldBeSameAs(TestExpressionExecutionDecorator.Step7_Result);

            decorator.AssertAllMethodsInvokedExacltyOnce();
        }

        [Fact]
        public void Should_apply_remote_expression_preparation_decorator()
        {
            var customExpression1 = new ConstantExpression("exp1");
            var customExpression2 = new ConstantExpression("exp2");

            var callCounter = new int[3];

            var decorator = new TestExpressionExecutionDecorator(new DefaultExpressionExecutor(null));

            decorator
                .With(x =>
                {
                    callCounter[0]++;
                    x.ShouldBeSameAs(TestExpressionExecutionDecorator.Step1_Expression);
                    return customExpression1;
                })
                .With(x =>
                {
                    callCounter[1]++;
                    x.ShouldBeSameAs(customExpression1);
                    return customExpression2;
                })
                .With(x =>
                {
                    callCounter[2]++;
                    x.ShouldBeSameAs(customExpression2);
                    return TestExpressionExecutionDecorator.Step1_Expression;
                })
                .Execute(TestExpressionExecutionDecorator.Step0_Expression)
                .ShouldBeSameAs(TestExpressionExecutionDecorator.Step7_Result);

            decorator.AssertAllMethodsInvokedExacltyOnce();
            callCounter.ShouldAllBe(x => x == 1);
        }

        [Fact]
        public void Should_replace_expression_transformation_decorator()
        {
            var callCounter = new int[1];

            var decorator = new TestExpressionExecutionDecorator(new DefaultExpressionExecutor(null));

            decorator
                .With(new Func<Expression, System.Linq.Expressions.Expression>(x =>
                {
                    throw new Exception("should have been replaced by next strategy");
                }))
                .With(new Func<Expression, System.Linq.Expressions.Expression>(x =>
                {
                    throw new Exception("should have been replaced by next strategy");
                }))
                .With((Expression x) =>
                {
                    callCounter[0]++;
                    x.ShouldBeSameAs(TestExpressionExecutionDecorator.Step1_Expression);
                    return TestExpressionExecutionDecorator.Step2_Expression;
                })
                .Execute(TestExpressionExecutionDecorator.Step0_Expression)
                .ShouldBeSameAs(TestExpressionExecutionDecorator.Step7_Result);

            decorator.AssertAllMethodsInvokedExacltyOnce(skip: 1);
            callCounter.ShouldAllBe(x => x == 1);
        }

        [Fact]
        public void Should_apply_system_expression_preparation_decorator()
        {
            var customExpression1 = System.Linq.Expressions.Expression.Constant("exp1");
            var customExpression2 = System.Linq.Expressions.Expression.Constant("exp2");

            var callCounter = new int[3];

            var decorator = new TestExpressionExecutionDecorator(new DefaultExpressionExecutor(null));

            decorator
                .With((System.Linq.Expressions.Expression x) =>
                {
                    callCounter[0]++;
                    x.ShouldBeSameAs(TestExpressionExecutionDecorator.Step3_Expression);
                    return customExpression1;
                })
                .With((System.Linq.Expressions.Expression x) =>
                {
                    callCounter[1]++;
                    x.ShouldBeSameAs(customExpression1);
                    return customExpression2;
                })
                .With((System.Linq.Expressions.Expression x) =>
                {
                    callCounter[2]++;
                    x.ShouldBeSameAs(customExpression2);
                    return TestExpressionExecutionDecorator.Step3_Expression;
                })
                .Execute(TestExpressionExecutionDecorator.Step0_Expression)
                .ShouldBeSameAs(TestExpressionExecutionDecorator.Step7_Result);

            decorator.AssertAllMethodsInvokedExacltyOnce();
            callCounter.ShouldAllBe(x => x == 1);
        }

        [Fact]
        public void Should_replace_expression_execution_decorator()
        {
            var callCounter = new int[1];

            var decorator = new TestExpressionExecutionDecorator(new DefaultExpressionExecutor(null));

            decorator
                .With(new Func<System.Linq.Expressions.Expression, object>(x =>
                {
                    throw new Exception("should have been replaced by next strategy");
                }))
                .With(new Func<System.Linq.Expressions.Expression, object>(x =>
                {
                    throw new Exception("should have been replaced by next strategy");
                }))
                .With((System.Linq.Expressions.Expression x) =>
                {
                    callCounter[0]++;
                    x.ShouldBeSameAs(TestExpressionExecutionDecorator.Step3_Expression);
                    return TestExpressionExecutionDecorator.Step4_Result;
                })
                .Execute(TestExpressionExecutionDecorator.Step0_Expression)
                .ShouldBeSameAs(TestExpressionExecutionDecorator.Step7_Result);

            decorator.AssertAllMethodsInvokedExacltyOnce(skip: 3);
            callCounter.ShouldAllBe(x => x == 1);
        }

        [Fact]
        public void Should_apply_raw_result_processing_decorator()
        {
            var customResult1 = "result1";
            var customResult2 = "result2";

            var callCounter = new int[3];

            var decorator = new TestExpressionExecutionDecorator(new DefaultExpressionExecutor(null));

            decorator
                .With((object x) =>
                {
                    callCounter[0]++;
                    x.ShouldBeSameAs(TestExpressionExecutionDecorator.Step5_Result);
                    return customResult1;
                })
                .With((object x) =>
                {
                    callCounter[1]++;
                    x.ShouldBeSameAs(customResult1);
                    return customResult2;
                })
                .With((object x) =>
                {
                    callCounter[2]++;
                    x.ShouldBeSameAs(customResult2);
                    return TestExpressionExecutionDecorator.Step5_Result;
                })
                .Execute(TestExpressionExecutionDecorator.Step0_Expression)
                .ShouldBeSameAs(TestExpressionExecutionDecorator.Step7_Result);

            decorator.AssertAllMethodsInvokedExacltyOnce();
            callCounter.ShouldAllBe(x => x == 1);
        }

        [Fact]
        public void Should_replace_result_to_dynamic_object_projection_decorator()
        {
            var callCounter = new int[1];

            var decorator = new TestExpressionExecutionDecorator(new DefaultExpressionExecutor(null));

            decorator
                .With(new Func<object, IEnumerable<DynamicObject>>(x =>
                {
                    throw new Exception("should have been replaced by next strategy");
                }))
                .With(new Func<object, IEnumerable<DynamicObject>>(x =>
                {
                    throw new Exception("should have been replaced by next strategy");
                }))
                .With((object x) =>
                {
                    callCounter[0]++;
                    x.ShouldBeSameAs(TestExpressionExecutionDecorator.Step5_Result);
                    return TestExpressionExecutionDecorator.Step6_Result;
                })
                .Execute(TestExpressionExecutionDecorator.Step0_Expression)
                .ShouldBeSameAs(TestExpressionExecutionDecorator.Step7_Result);

            decorator.AssertAllMethodsInvokedExacltyOnce(skip: 5);
            callCounter.ShouldAllBe(x => x == 1);
        }

        [Fact]
        public void Should_apply_dynamic_object_result_processing_decorator()
        {
            var customResult1 = new[] { new DynamicObject("result1") };
            var customResult2 = new[] { new DynamicObject("result2") };

            var callCounter = new int[3];

            var decorator = new TestExpressionExecutionDecorator(new DefaultExpressionExecutor(null));

            decorator
                .With((IEnumerable<DynamicObject> x) =>
                {
                    callCounter[0]++;
                    x.ShouldBeSameAs(TestExpressionExecutionDecorator.Step7_Result);
                    return customResult1;
                })
                .With((IEnumerable<DynamicObject> x) =>
                {
                    callCounter[1]++;
                    x.ShouldBeSameAs(customResult1);
                    return customResult2;
                })
                .With((IEnumerable<DynamicObject> x) =>
                {
                    callCounter[2]++;
                    x.ShouldBeSameAs(customResult2);
                    return TestExpressionExecutionDecorator.Step7_Result;
                })
                .Execute(TestExpressionExecutionDecorator.Step0_Expression)
                .ShouldBeSameAs(TestExpressionExecutionDecorator.Step7_Result);

            decorator.AssertAllMethodsInvokedExacltyOnce();
            callCounter.ShouldAllBe(x => x == 1);
        }
    }
}
