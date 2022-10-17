create or alter procedure Task4 @sum_int bigint output,
                       @median_float float output
as
begin
    set @sum_int = (select sum(cast(RandomInteger as bigint)) from Task1);
    set @median_float = (select avg(RandomFloat) from Task1);
end
go

declare @sum bigint, @med float
exec Task4 @sum output , @med output 
select @sum, @med
go
