USE [ChatDB]
GO

INSERT INTO [dbo].[Users]
           ([Login]
           ,[Name]
           ,[Password]
           ,[Salt]
           ,[RoomName])
     VALUES
           ('user1'
           ,'user1'
           ,0xA2015B13E5FE81D7C0F3ADAEABED603B9A9F59DAF9B6D1F872DC05AAFFE4AC2E90BF35D39AD9A23BF0D5697C0E515BA8F11758FE5EF215C3CB3DFF273EB853C9
           ,0x56756FF643EB40DFFFC5812379A8C81256117E9979C993B0A0BFB3B251F6C4CF23570003B0758D48F9752E8726DDD85F31B7A13CCE89687101AAB0C9DCEFD3894D25565FD4B4228CB716FDE722DB30C9A9AB1B67788EFDC0717A6E04460F389D75A3502D42B42D28AEB8D88FD2158E4DB8870E140506B135B6D8BA77F1A481E3
           ,null)

INSERT INTO [dbo].[Users]
           ([Login]
           ,[Name]
           ,[Password]
           ,[Salt]
           ,[RoomName])
     VALUES
           ('user2'
           ,'user2'
           ,0xA2015B13E5FE81D7C0F3ADAEABED603B9A9F59DAF9B6D1F872DC05AAFFE4AC2E90BF35D39AD9A23BF0D5697C0E515BA8F11758FE5EF215C3CB3DFF273EB853C9
           ,0x56756FF643EB40DFFFC5812379A8C81256117E9979C993B0A0BFB3B251F6C4CF23570003B0758D48F9752E8726DDD85F31B7A13CCE89687101AAB0C9DCEFD3894D25565FD4B4228CB716FDE722DB30C9A9AB1B67788EFDC0717A6E04460F389D75A3502D42B42D28AEB8D88FD2158E4DB8870E140506B135B6D8BA77F1A481E3
           ,null)
GO


