using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

public class LoadScenes : MonoBehaviour
{
	private List<SaveEntry> saveEntries = null;
	public List<SaveEntry> SaveEntries { get { return saveEntries; } }
	
	// Use this for initialization
	void Start()
	{
		// get save files
		saveEntries = DeserializeSaveEntries( GetSaveGameNames() );
	}
	
	/// <summary>
	/// Returns the array of file path names
	/// </summary>
	private string[] GetSaveGameNames()
	{
		return Directory.GetFiles( Application.persistentDataPath, "Sandplay_Save*" );
	}
	
	/// <summary>
	/// Returns the list of SaveEntry game objects
	/// </summary>
	/// <param name="paths"></param>
	private List<SaveEntry> DeserializeSaveEntries( string[] paths )
	{
		// create serializer
		XmlSerializer deserializer = new XmlSerializer( typeof( SaveEntry ) );
		
		// create internal temp variables
		SaveEntry saveEntry = null;
		List<SaveEntry> saveEntries = new List<SaveEntry>();
		
		for ( int i = 0; i < paths.Length; i++ )
		{
			// read file and deserialize it into object
			using ( StreamReader reader = new StreamReader( paths[ i ] ) )
			{
				saveEntry = ( SaveEntry )deserializer.Deserialize( reader );
			}
			saveEntries.Add( saveEntry );
		}
		
		return saveEntries;
	}
	
	
	/// <summary>
	/// Returns image based on Safe Entry
	/// </summary>
	public Texture2D GetTexture( SaveEntry entry )
	{
		Texture2D texture = null;
		//Open the stream and read it back. 
		using ( FileStream fs = File.OpenRead( entry._screenshot_filename ) )
		{
			byte[] imageData = new byte[ fs.Length ];
			fs.Read( imageData, 0, ( int )fs.Length );
			
			// getting texture size before loading texture
			float width;
			float height;
			GetPNGTextureSize( imageData, out width, out height );
			
			texture = new Texture2D( (int)width, (int)height );
			texture.LoadImage( imageData );
		}
		return texture;
	}
	
	/// <summary>
	/// Loads the scene
	/// </summary>
	public void StartLoad( SaveEntry save )
	{
		SaveScene._save_game = save;
		Application.LoadLevel( 2 );
	}
	
	
	
	/// <summary>
	/// Not mine pasted from http://denis-potapenko.blogspot.com.au/2013/10/task-7-getting-size-of-png-textures.html
	/// Checks the size of the texture before it is being loaded
	/// </summary>
	private void GetPNGTextureSize( byte[] bytes, out float width, out float height )
	{
		width = -1.0f;
		height = -1.0f;
		
		// check only png tex!!! // http://www.libpng.org/pub/png/spec/1.2/PNG-Structure.html
		byte[] png_signature = { 137, 80, 78, 71, 13, 10, 26, 10 };
		
		const int cMinDownloadedBytes = 30;
		byte[] buf = bytes;
		if ( buf.Length > cMinDownloadedBytes )
		{
			// now we can check png format
			for ( int i = 0; i < png_signature.Length; i++ )
			{
				if ( buf[ i ] != png_signature[ i ] )
				{
					Debug.LogWarning( "Error! Texture os NOT png format!" );
					return; // this is NOT png file!
				}
			}
			
			// now get width and height of texture
			width = buf[ 16 ] << 24 | buf[ 17 ] << 16 | buf[ 18 ] << 8 | buf[ 19 ];
			height = buf[ 20 ] << 24 | buf[ 21 ] << 16 | buf[ 22 ] << 8 | buf[ 23 ];
			
			Debug.Log( "Loaded texture size: width = " + width + "; height = " + height );
			return;
		}
	}
}
