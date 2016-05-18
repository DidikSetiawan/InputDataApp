create procedure getData
as
select * from datakomputer
go

create procedure getDataCari @namauser nvarchar(30)
as
select * from datakomputer where NamaUser = @namauser
go

create procedure upDataK @namauser nvarchar(30), @ipaddr nvarchar(30), @ket nvarchar(30)
as
insert into datakomputer (NamaUser,IP_Address,Deskripsi) values (@namauser,@ipaddr,@ket)
go

create procedure upDataEdit @namauser nvarchar(30), @ipaddr nvarchar(30), @ket nvarchar(30)
as
update datakomputer set NamaUser = @namauser, IP_Address = @ipaddr, Deskripsi = @ket where NamaUser = @namauser
go

create procedure upDataDel @namauser nvarchar(30)
as
delete from datakomputer where NamaUser = @namauser
go