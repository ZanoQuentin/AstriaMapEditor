<?php
if($_SERVER['HTTP_ACCEPT_LANGUAGE']!='')
	exit;
	
/*  -------
   | MySQL |
    -------    */
define('MYSQL_HOST', ''); 		// Ip/Host du Serveur Sql
define('MYSQL_USER', ''); 				// Nom d'utilisateur
define('MYSQL_PASS', ''); 		// Mot de passe 
define('MYSQL_DB'  , ''); 		// Nom de la base de données du site

// Connexion AME
try{
	$pdo_options[PDO::ATTR_ERRMODE] = PDO::ERRMODE_EXCEPTION;
	$BDD_CMS = @new PDO('mysql:host='.MYSQL_HOST.';dbname='.MYSQL_DB, MYSQL_USER, MYSQL_PASS, $pdo_options);
} catch (Exception $e) { echo 'Maintenance'; exit; }
$BDD_CMS->query("SET NAMES UTF8");


// Check account
$id=0;
if(isset($_GET['hwid'])) {
	// Connexions by HWID
	$requete = $BDD_CMS->prepare('SELECT * FROM accounts WHERE hwid=:hwid;');
	$requete->bindValue('hwid', $_GET['hwid'], PDO::PARAM_STR);
	$requete->execute();
	$data = $requete->fetch();
	if(!isset($data['id'])) {
		// First Connexion
		if(isset($_GET['pseudo']) AND isset($_GET['pass'])) {
			$requete = $BDD_CMS->prepare('SELECT * FROM accounts WHERE pseudo=:login AND hwid=\'\';');
			$requete->bindValue('login', $_GET['pseudo'], PDO::PARAM_STR);
			$requete->execute();
			
			$data2 = $requete->fetch();
			if ($data2['password'] == $_GET['pass']) {
				$update = $BDD_CMS->prepare("UPDATE accounts SET hwid=:hwid WHERE pseudo=:login;");
				$update->bindValue('hwid', $_GET['hwid'], PDO::PARAM_STR);
				$update->bindValue('login', $_GET['pseudo'], PDO::PARAM_STR);
				$update->execute();
				
				$id = $data2['id'];
			} else {
				echo '0'; exit;
			}
			$requete->closeCursor();
		} else {
			exit;
		}
	} else { $id = $data['id']; }
	$requete->closeCursor();
} else {
	exit;
}

$requete = $BDD_CMS->prepare('SELECT * FROM accounts WHERE hwid=:hwid;');
$requete->bindValue('hwid', $_GET['hwid'], PDO::PARAM_STR);
$requete->execute();
$data = $requete->fetch();
// 		Variables chargées depuis la BDD
$host_sql2 = $data['bdd_host'];						// Ip/Host du Serveur Sql
$user2_sql = $data['bdd_user'];						// Nom d'utilisateur
$mdp2_sql = $data['bdd_pass']; 						// Mot de passe 
$db_Other_sql = $data['bdd_database']; 				// Nom de la base de données du site
// Table config
$maps = $data['bdd_table_maps'];
$areas = $data['bdd_table_areas'];
$subareas = $data['bdd_table_subareas'];
$monsters = $data['bdd_table_monsters'];

$tilesRequired = $data['packs'];
$tilesRequireds = explode('|', $tilesRequired);
$requete->closeCursor();

// Connexion Dofus
try{
	$pdo_options[PDO::ATTR_ERRMODE] = PDO::ERRMODE_EXCEPTION;
	$BDD_Other = @new PDO('mysql:host='.$host_sql2.';dbname='.$db_Other_sql, $user2_sql, $mdp2_sql, $pdo_options);
} catch (Exception $e) { echo 'Maintenance'; exit; }
$BDD_Other->query("SET NAMES UTF8");

// Get Datas
if(isset($_GET['xml'])) {

	if($_GET['xml']=='grounds' or $_GET['xml']=='objects') {
		$file = '<?xml version="1.0"?><ArrayOfPos xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">';
	
		$count = count($tilesRequireds);
		if($tilesRequired != '') {
			if($_GET['xml']=='grounds') {
				$cmdTiles = ' WHERE';
				for($i=0; $i<$count; $i++) {
					$cmdTiles.=' type=0 AND pack=\''.$tilesRequireds[$i].'\' OR';
				}
				$cmdTiles = substr($cmdTiles, 0, strlen($cmdTiles)-2);
			} elseif($_GET['xml']=='objects') {
				$cmdTiles = ' WHERE';
				for($i=0; $i<$count; $i++) {
					$cmdTiles.=' type=1 AND pack=\''.$tilesRequireds[$i].'\' OR';
				}
				$cmdTiles = substr($cmdTiles, 0, strlen($cmdTiles)-2);
			}
			$Tiles = $BDD_CMS->query('SELECT * FROM tiles'.$cmdTiles.';');
			
			while($donnees = $Tiles->fetch()) {
				$file.= '<Pos><ID>'.$donnees['id_tile'].'</ID><X>'.$donnees['x'].'</X><Y>'.$donnees['y'].'</Y></Pos>';
			}
			$Tiles->closeCursor();
		}
		
		$file.= '</ArrayOfPos>';
		
	} elseif($_GET['xml']=='areas') {
		$file = '<?xml version="1.0"?><ArrayOfArea xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">';
	
		$Tiles = $BDD_Other->query('SELECT * FROM '.$areas.';');
		while($donnees = $Tiles->fetch()) {
			$file.= '<Area><ID>'.$donnees['id'].'</ID><Name>'.utf8_decode($donnees['name']).'</Name><SuperArea>'.$donnees['superarea'].'</SuperArea></Area>';
		}
		$Tiles->closeCursor();
		
		$file.= '</ArrayOfArea>';
		
	} elseif($_GET['xml']=='subareas') {
		$file = '<?xml version="1.0"?><ArrayOfSubArea xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">';
	
		$Tiles = $BDD_Other->query('SELECT * FROM '.$subareas.';');
		while($donnees = $Tiles->fetch()) {
			$file.= '<SubArea><ID>'.$donnees['id'].'</ID><Name>'.utf8_decode($donnees['name']).'</Name><Area>'.$donnees['area'].'</Area></SubArea>';
		}
		$Tiles->closeCursor();
		
		$file.= '</ArrayOfSubArea>';
		
	} elseif($_GET['xml']=='monsters') {
		$file = '<?xml version="1.0"?><ArrayOfMonster xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">';
	
		$Tiles = $BDD_Other->query('SELECT * FROM '.$monsters.';');
		while($donnees = $Tiles->fetch()) {
			$file.= '<Monster><ID>'.$donnees['id'].'</ID><Name>'.utf8_decode($donnees['name']).'</Name><GfxID>'.$donnees['gfxID'].'</GfxID><Grades>'.$donnees['grades'].'</Grades><Pdvs>'.$donnees['pdvs'].'</Pdvs><Points>'.$donnees['points'].'</Points><Initiative>'.$donnees['inits'].'</Initiative><MinKamas>'.$donnees['minKamas'].'</MinKamas><MaxKamas>'.$donnees['maxKamas'].'</MaxKamas><Experience>'.$donnees['exps'].'</Experience></Monster>';
		}
		$Tiles->closeCursor();
		
		$file.= '</ArrayOfMonster>';
	}

	echo base64_encode($file);

}
if(isset($_GET['get'])) {
	$file = '';

	if($_GET['get']=='maps') {
		$Maps = $BDD_Other->query('SELECT * FROM '.$maps.' ORDER BY id DESC;');
		while($donnees = $Maps->fetch()) {
			$file.= $donnees['id'].','.$donnees['date'].'|';
		}
		$Maps->closeCursor();
	} elseif($_GET['get']=='map' and isset($_GET['id'])) {
		$Maps = $BDD_Other->query('SELECT * FROM '.$maps.' WHERE id='.intval($_GET['id']).';');
		$donnees = $Maps->fetch();
		$file.= $donnees['places'].'!'.$donnees['key'].'!'.$donnees['monsters'].'!'.$donnees['mappos'].'!'.$donnees['numgroup'].'!'.$donnees['groupmaxsize'];
		$Maps->closeCursor();
	}
	
	echo base64_encode($file);

}
if(isset($_GET['test'])) echo '1';