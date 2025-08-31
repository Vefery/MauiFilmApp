-- Получить фильмы, в которых снимались актеры, 
-- имя или фамилия которых начинается на "Госл"
SELECT Films.Id, Films.Name, Films.PosterPath FROM Films
INNER JOIN FeaturedActors ON Films.Id = FeaturedActors.FilmId
INNER JOIN Actors ON FeaturedActors.ActorId = Actors.Id
WHERE Actors.NameNormalized like 'ГОСЛ%' OR Actors.NameNormalized like '% ГОСЛ%'

-- Получить фильмы, в которых названии которых 
-- есть "тяжкие"
SELECT Films.Id, Films.Name, Films.PosterPath FROM Films
WHERE Films.NameNormalized like '%ТЯЖКИЕ%'

-- Получить фильмы с жанрами драма и сериал
SELECT Films.Id, Films.Name, Films.PosterPath FROM Films
INNER JOIN HasGenre ON Films.Id = HasGenre.FilmId
INNER JOIN Genres ON HasGenre.GenreId = Genres.Id
WHERE Genres.GenreName IN ('Драма', 'Сериал')
GROUP BY Films.Id
HAVING count(DISTINCT Genres.Id) = 2

-- Получить фильмы с жанрами драма и сериал, в 
-- которых снимались актеры, имя или фамилия
-- которых начинается на "Госл"
SELECT Films.Id, Films.Name, Films.PosterPath FROM Films
INNER JOIN HasGenre ON Films.Id = HasGenre.FilmId
INNER JOIN Genres ON HasGenre.GenreId = Genres.Id
INNER JOIN FeaturedActors ON Films.Id = FeaturedActors.FilmId
INNER JOIN Actors ON FeaturedActors.ActorId = Actors.Id
WHERE (Actors.NameNormalized like 'ГОСЛ%' OR Actors.NameNormalized like '% ГОСЛ%') AND Genres.GenreName IN ('Криминал', 'Триллер')
GROUP BY Films.Id
HAVING count(DISTINCT Genres.Id) = 2

-- Получить фильмы с жанрами триллер и фантастика, в 
-- которых названии которых есть "по"
SELECT Films.Id, Films.Name, Films.PosterPath FROM Films
INNER JOIN HasGenre ON Films.Id = HasGenre.FilmId
INNER JOIN Genres ON HasGenre.GenreId = Genres.Id
WHERE (Films.NameNormalized like '%ПО%') AND Genres.GenreName IN ('Триллер', 'Фантастика')
GROUP BY Films.Id
HAVING count(DISTINCT Genres.Id) = 2