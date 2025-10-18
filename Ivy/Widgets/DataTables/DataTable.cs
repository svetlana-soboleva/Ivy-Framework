using Ivy.Core;
using Ivy.Shared;

// ReSharper disable once CheckNamespace
namespace Ivy;

public record DataTable : WidgetBase<DataTable>
{
    public DataTable(
        DataTableConnection connection,
        Size? width,
        Size? height,
        DataTableColumn[] columns,
        DataTableConfiguration configuration
    )
    {
        Width = width ?? Size.Full();
        Height = height ?? Size.Full();
        Connection = connection;
        Columns = columns;
        Configuration = configuration;
    }

    [Prop] public DataTableColumn[] Columns { get; set; }

    [Prop] public DataTableConnection Connection { get; set; }

    [Prop] public DataTableConfiguration Configuration { get; set; }

    public static Detail operator |(DataTable widget, object child)
    {
        throw new NotSupportedException("DataTable does not support children.");
    }
}