using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class ItemGetSp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE [dbo].[GetItemsWithPagination]
                    @PageIndex INT,
                    @PageSize INT,
                    @OrderBy NVARCHAR(50),
                    @Name NVARCHAR(MAX) = '%',
                    @ProductCode NVARCHAR(MAX) = '%',
                    @ItemType NVARCHAR(MAX) = '%',
                    @Category NVARCHAR(MAX) = '%',
                    @Total INT OUTPUT,
                    @TotalDisplay INT OUTPUT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    -- Collect Total count of items (unfiltered)
                    SELECT @Total = COUNT(*) FROM Items;

                    -- Create a temporary table to store category values
                    IF OBJECT_ID('tempdb..#CategoryTable') IS NOT NULL
                        DROP TABLE #CategoryTable;

                    CREATE TABLE #CategoryTable (Value NVARCHAR(MAX));

                    -- Populate the table with values split from the @Category parameter
                    DECLARE @start INT = 1, @end INT, @length INT = LEN(@Category);

                    WHILE @start <= @length
                    BEGIN
                        SET @end = CHARINDEX(',', @Category, @start);
                        IF @end = 0 
                            SET @end = @length + 1;

                        INSERT INTO #CategoryTable (Value)
                        VALUES (SUBSTRING(@Category, @start, @end - @start));

                        SET @start = @end + 1;
                    END

                    -- Build the dynamic SQL for collecting Total Display based on filters
                    DECLARE @countSQL NVARCHAR(2000) = 'SELECT @TotalDisplay = COUNT(*) FROM Items WHERE 1 = 1';

                    -- Applying filters
                    IF @Name <> '%' 
                        SET @countSQL = @countSQL + ' AND Name LIKE @xName';

                    IF @ProductCode <> '%' 
                        SET @countSQL = @countSQL + ' AND ProductCode LIKE @xProductCode';

                    IF @ItemType <> '%' 
                        SET @countSQL = @countSQL + ' AND ItemType LIKE @xItemType';

                    IF @Category <> '%'
                        -- Use LIKE to handle cases like "Electronics, Computers"
                        SET @countSQL = @countSQL + ' AND (Category LIKE ''%' + @Category + '%'' OR Category IN (SELECT Value FROM #CategoryTable))';

                    -- Defining the parameter list for the count query
                    DECLARE @countParamList NVARCHAR(500) = '@xName NVARCHAR(MAX), @xProductCode NVARCHAR(MAX), @xItemType NVARCHAR(MAX), @TotalDisplay INT OUTPUT';

                    -- Executing the count query to calculate Total Display (filtered items count)
                    EXEC sp_executesql @countSQL, @countParamList,
                        @xName = @Name,
                        @xProductCode = @ProductCode,
                        @xItemType = @ItemType,
                        @TotalDisplay = @TotalDisplay OUTPUT;

                    -- Build the dynamic SQL for fetching paginated results based on filters
                    DECLARE @sql NVARCHAR(2000) = 'SELECT Id, Name, ProductCode, ItemType, Category, OpeningStock, UnitOfMeasure, Image
                                                    FROM Items WHERE 1 = 1';

                    -- Applying filters for the main data query
                    IF @Name <> '%'
                        SET @sql = @sql + ' AND Name LIKE @xName';

                    IF @ProductCode <> '%'
                        SET @sql = @sql + ' AND ProductCode LIKE @xProductCode';

                    IF @ItemType <> '%'
                        SET @sql = @sql + ' AND ItemType LIKE @xItemType';

                    IF @Category <> '%'
                        -- Use LIKE to handle cases like "Electronics, Computers"
                        SET @sql = @sql + ' AND (Category LIKE ''%' + @Category + '%'' OR Category IN (SELECT Value FROM #CategoryTable))';

                    -- Applying sorting and pagination
                    SET @sql = @sql + ' ORDER BY ' + @OrderBy + ' OFFSET @PageSize * (@PageIndex - 1) ROWS FETCH NEXT @PageSize ROWS ONLY';

                    -- Defining the parameter list for the main query
                    DECLARE @paramList NVARCHAR(500) = '@xName NVARCHAR(MAX), @xProductCode NVARCHAR(MAX), @xItemType NVARCHAR(MAX), @PageIndex INT, @PageSize INT';

                    -- Executing the main query
                    EXEC sp_executesql @sql, @paramList,
                        @xName = @Name,
                        @xProductCode = @ProductCode,
                        @xItemType = @ItemType,
                        @PageIndex = @PageIndex,
                        @PageSize = @PageSize;

                    -- Drop the temporary table after use
                    DROP TABLE #CategoryTable;

                END
                
                
                

                """;

            migrationBuilder.Sql(sql);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetItemsWithPagination]";
            migrationBuilder.DropTable(sql);

        }
    }
}
