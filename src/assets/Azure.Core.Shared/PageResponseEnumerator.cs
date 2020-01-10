﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Azure.Core
{
    internal static class PageResponseEnumerator
    {
        public static FuncPageable<T> CreateEnumerable<T>(Func<string?, Page<T>> pageFunc) where T : notnull
        {
            return new FuncPageable<T>((continuationToken, pageSizeHint) => pageFunc(continuationToken));
        }

        public static FuncPageable<T> CreateEnumerable<T>(Func<string?, int?, Page<T>> pageFunc) where T : notnull
        {
            return new FuncPageable<T>(pageFunc);
        }

        public static AsyncPageable<T> CreateAsyncEnumerable<T>(Func<string?, Task<Page<T>>> pageFunc) where T : notnull
        {
            return new FuncAsyncPageable<T>((continuationToken, pageSizeHint) => pageFunc(continuationToken));
        }

        public static AsyncPageable<T> CreateAsyncEnumerable<T>(Func<string?, int?, Task<Page<T>>> pageFunc) where T : notnull
        {
            return new FuncAsyncPageable<T>((continuationToken, pageSizeHint) => pageFunc(continuationToken, pageSizeHint));
        }

        public static AsyncPageable<T> CreateAsyncEnumerable<T>(Func<Task<Page<T>>> firstPageFunc, Func<string?, Task<Page<T>>> nextPageFunc) where T : notnull
        {
            return new FuncAsyncPageable<T>((continuationToken, pageSizeHint) => firstPageFunc(), (continuationToken, pageSizeHint) => nextPageFunc(continuationToken));
        }

        public static AsyncPageable<T> CreateAsyncEnumerable<T>(Func<int?, Task<Page<T>>> firstPageFunc, Func<string?, int?, Task<Page<T>>> nextPageFunc, int? pageSize = default) where T : notnull
        {
            return new FuncAsyncPageable<T>((continuationToken, pageSizeHint) => firstPageFunc(pageSizeHint), (continuationToken, pageSizeHint) => nextPageFunc(continuationToken, pageSizeHint), pageSize);
        }

        internal delegate Task<Page<T>> AsyncPageFunc<T>(string? continuationToken = default, int? pageSizeHint = default);

        internal class FuncAsyncPageable<T> : AsyncPageable<T> where T : notnull
        {
            private readonly AsyncPageFunc<T> _firstPageFunc;
            private readonly AsyncPageFunc<T> _nextPageFunc;
            private readonly int? _defaultPageSize;

            public FuncAsyncPageable(AsyncPageFunc<T> pageFunc, int? defaultPageSize = default)
            {
                _firstPageFunc = pageFunc;
                _nextPageFunc = pageFunc;
                _defaultPageSize = defaultPageSize;
            }

            public FuncAsyncPageable(AsyncPageFunc<T> firstPageFunc, AsyncPageFunc<T> nextPageFunc, int? defaultPageSize = default)
            {
                _firstPageFunc = firstPageFunc;
                _nextPageFunc = nextPageFunc;
                _defaultPageSize = defaultPageSize;
            }

            public override async IAsyncEnumerable<Page<T>> AsPages(string? continuationToken = default, int? pageSizeHint = default)
            {
                AsyncPageFunc<T> pageFunc = _firstPageFunc;
                int? pageSize = pageSizeHint ?? _defaultPageSize;
                do
                {
                    Page<T> pageResponse = await pageFunc(continuationToken, pageSize).ConfigureAwait(false);
                    yield return pageResponse;
                    continuationToken = pageResponse.ContinuationToken;
                    pageFunc = _nextPageFunc;
                } while (continuationToken != null);
            }
        }

        internal class FuncPageable<T> : Pageable<T> where T : notnull
        {
            private readonly Func<string?, int?, Page<T>> _pageFunc;

            public FuncPageable(Func<string?, int?, Page<T>> pageFunc)
            {
                _pageFunc = pageFunc;
            }

            public override IEnumerable<Page<T>> AsPages(string? continuationToken = default, int? pageSizeHint = default)
            {
                do
                {
                    Page<T> pageResponse = _pageFunc(continuationToken, pageSizeHint);
                    yield return pageResponse;
                    continuationToken = pageResponse.ContinuationToken;
                } while (continuationToken != null);
            }
        }
    }
}
