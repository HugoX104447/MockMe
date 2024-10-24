﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockMe.Mocks;

public interface IMemberMock<TReturn, TSelf> : IVoidMemberMock<TSelf>
{
    public TSelf Return(TReturn returnThis, params TReturn[] thenReturnThese);
}

public interface IVoidMemberMock<TSelf>
{
    public TSelf Callback(Action callback);
}

public interface IMemberMockWithArgs<TSelf, TCallback> : IVoidMemberMock<TSelf>
{
    public TSelf Callback(TCallback callback);
}

public interface IMemberMockWithArgs<TReturn, TSelf, TCallback, TReturnCall>
    : IMemberMock<TReturn, TSelf>
{
    public TSelf Return(TReturnCall returnThis, params TReturnCall[] thenReturnThese);
    public TSelf Callback(TCallback callback);
}
