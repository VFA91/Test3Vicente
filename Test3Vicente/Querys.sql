--Listar todos los eventos con la cantidad de invitados correspondiente
SELECT [Description],
       [Date],
       Ci.Name,
  (SELECT count(*)
   FROM [EventManagement].[dbo].[GuestEvent]
   WHERE EventId = Ev.EventId) AS NumeroInvitados
FROM [EventManagement].[dbo].[Event] AS Ev
INNER JOIN [EventManagement].[dbo].[City] AS Ci ON Ev.CityId = Ci.CityId
  
--Listar todas las ciudades que tengan invitados asistiendo a más de un evento
  
SELECT DISTINCT ci.Name
FROM [EventManagement].[dbo].[Event] AS Ev
INNER JOIN [EventManagement].[dbo].GuestEvent AS GuEv ON Ev.EventId = GuEv.EventId
INNER JOIN [EventManagement].[dbo].[City] Ci ON ci.CityId = Ev.CityId
WHERE GuEv.GuestId IN
    ( SELECT G.GuestId
     FROM [EventManagement].[dbo].[Guest] G
     INNER JOIN [EventManagement].[dbo].[GuestEvent] Ge ON G.GuestId = Ge.GuestId
     GROUP BY G.GuestId
     HAVING Count(G.GuestId) > 1)

--Listar todos los servicios prestados en una ciudad indicando la cantidad de invitados que van a consumir cada servicio

select S.Name, C.Name, count(G.GuestId) as Guest from EventManagement.dbo.Event E
inner join EventManagement.dbo.City C
ON E.CityId = C.CityId
inner join EventManagement.dbo.EventService EvSe
ON EvSe.EventId= E.EventId
inner join EventManagement.dbo.Service S
on S.ServiceId = EvSe.ServiceId
inner join EventManagement.dbo.GuestEvent GeEv
on GeEv.EventId = E.EventId
inner join EventManagement.dbo.Guest G
on G.GuestId = GeEv.GuestId
inner join EventManagement.dbo.GuestService GeSe
on GeSe.GuestId = G.GuestId and GeSe.ServiceId = s.ServiceId
--where C.CityId = 1
group by S.Name, C.Name

--Listar todos los nombres de los invitados y las fechas de los eventos en los cuales están inscriptos, considerar el caso de invitados sin eventos.

SELECT CONCAT([FirstName] + ' ',[LastName]) AS NombreCompleto ,
       E.Date
FROM [EventManagement].[dbo].[Guest] G
LEFT JOIN [EventManagement].[dbo].[GuestEvent] Ge ON G.GuestId = Ge.GuestId
LEFT JOIN [EventManagement].[dbo].[Event] E ON Ge.EventId = E.EventId

--Listar todas las ciudades de modelo, indicando el margen total que se obtiene de todos los eventos programados para cada ciudad.

select SUM((S.PVP - S.CUP)) as MargenTotal, C.Name from EventManagement.dbo.Event E
inner join EventManagement.dbo.City C
ON E.CityId = C.CityId
inner join EventManagement.dbo.EventService EvSe
ON EvSe.EventId= E.EventId
inner join EventManagement.dbo.Service S
on S.ServiceId = EvSe.ServiceId
inner join EventManagement.dbo.GuestEvent GeEv
on GeEv.EventId = E.EventId
group by C.Name