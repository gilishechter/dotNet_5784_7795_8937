using System;
using System.Collections;
using System.Collections.Generic;

namespace PL;
internal class Rankcollection : IEnumerable
{
    static readonly IEnumerable<BO.Rank> s_enums = (Enum.GetValues(typeof(BO.Rank)) as IEnumerable<BO.Rank>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
internal class Statuscollection : IEnumerable
{
    static readonly IEnumerable<BO.Status> s_enums = (Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
internal class Levelcollection : IEnumerable
{
    static readonly IEnumerable<BO.level> s_enums = (Enum.GetValues(typeof(BO.level)) as IEnumerable<BO.level>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class Filtercollection : IEnumerable
{
    static readonly IEnumerable<BO.filter> s_enums = (Enum.GetValues(typeof(BO.filter)) as IEnumerable<BO.filter>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
internal class StatusProjectcollection : IEnumerable
{
    static readonly IEnumerable<BO.StatusProject> s_enums = (Enum.GetValues(typeof(BO.StatusProject)) as IEnumerable<BO.StatusProject>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

