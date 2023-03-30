const addSongForm = document.getElementById("addSongForm")
const title = document.getElementById("title")
const artist = document.getElementById("artist")
const outPut = document.getElementById("songList")
const url = "https://localhost:7133/api/Songs"

let songs = []

async function handleOnLoad()
{
    songs = await getSongs()
    displaySongs(songs)
}

async function getSongs()
{
    //make fetch request to url to get the data from the webpage
    let response = await fetch(url)
    let result = await response.json()
    return result || []
}

async function addSong(song)
{
    //use the network request to add the song
    let response = await fetch(url, {
        //convert the song object to a JSON string
        body: JSON.stringify(song),
        headers: {
            "Content-Type": "application/json"
        },
        method: "post",   //make the request method post
    });
    //return the data in an array
    let result = await response.json();
    return result || []
}

async function updateSong(song)
{
    //update an existing song by appending the id
    let updateurl = `${url}/${song.songID}`
    let updateBody = JSON.stringify(song)
    let response = await fetch(updateurl, {
        body: updateBody,
        headers: {
            "Content-Type": "application/json"
        },
        method: "put",
    })
    let result = await response.json();
    return result || []
}

async function deleteSong(songID)
{
    let response = await fetch(`${url}/${songID}`, {
        method: "delete",
    })
}

addSongForm.addEventListener("submit", async(e) => {
    e.preventDefault()

    const song = {
        Title: title.value,
        Artist: artist.value,
        dateAdded: new Data().toISOString(),
        Favorite: false,
        Deleted: false
    }

    let newSong = await addSong(song)

    songs.push(newSong)
    title.value = ""
    artist.value = ""
    displaySongs(songs)
})

async function displaySongs(songsArray)
{
    const songContainer = document.querySelector("#songsArray")
    songContainer.innerHTML = ""

    for (let x = 0; x < songsArray.length; x++)
    {
        const song = songsArray[i]

        if(!song.deleted)
        {
            //create song id element
            const songIDElement = document.createElement(`h1`)
            songIDElement.innerText = song.songID

            //create title element
            const titleElement = document.createElement(`h1`)
            titleElement.innerText = song.title

            //create artist element
            const artistElement = document.createElement(`p`)
            artistElement.innerText = song.artist

            //create date added element 
            const dateElement = document.createElement(`p`)
            dateElement.innerText = song.dateAdded

            //create favorite element
            const favoriteElement = document.createElement(`p`)
            favoriteElement.innerText = song.favorite

            //create a box to check if the song is favorite or not
            const favoriteCheckBox = document.createElement("input")
            favoriteCheckBox.type = "checkbox"
            favoriteCheckBox.checked = song.favorite
            favoriteCheckBox.addEventListener("click", async () => {
                let songID = Number.parseInt(songIDElement.innerText)
                let title = titleElement.innerText
                let artist = artistElement.innerText
                let dateAdded = dateElement.innerText
                let favorite = favoriteCheckBox.checked
                let deleted = (deletedElement.innerText.toLowerCase() === "true")
                let song = {
                    songID: songID,
                    title: title,
                    artist: artist,
                    dateAdded: dateAdded,
                    favorite: favorite,
                    deleted: deleted
                }
                await updateSong(song)
                songs = await getSongs()
                displaySongs(songs)
            })

            const deletedElement = document.createElement(`p`)
            deletedElement.innerText = song.deleted

            //create delete button
            const deletedButton = document.createElement("button")
            deletedButton.innerText = "Delete Song"
            deletedButton.addEventListener("click", async (e) =>{
                e.preventDefault()
                let songID = Number.parseInt(songIDElement.innerText)
                await deleteSong(songID)
                songs = await getSongs()
                displaySongs(songs)
            })

            const songElement = document.createElementNS("div")

            songElement.appendChild(songIDElement)
            songElement.appendChild(titleElement)
            songElement.appendChild(artistElement)
            songElement.appendChild(dateAddedElement)
            songElement.appendChild(favoriteElement)
            songElement.appendChild(deletedElement)
            songElement.appendChild(deletedButton)

            songContainer.appendChild(songElement)
        }
    }
}
