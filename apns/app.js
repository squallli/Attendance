
var Connection = require('tedious').Connection;  
var config = {  
    userName: 'regalsa',  
    password: 'sA89947155',  
    server: '192.168.0.11'
    // options: {
    //     instanceName: 'SQLEXPRESS'
    // } 
};  
var connection = new Connection(config); 
var Request = require('tedious').Request;  
var insertRequest = require('tedious').Request;  
var TYPES = require('tedious').TYPES;  

var tp = require('tedious-promises');
tp.setConnectionConfig(config); 
process.setMaxListeners(0);


connection.on('connect', function(err) {  

    //executeStatement();
    
    sendNotification("000156","陳weiwei","1","20189999","9999","0207E89499C5D12AE79D3F07679F6FE9F99F1990F8E8F61576B543A33CB1505A");
});

function updateSql(columns)
{
    tp.sql("update [Attendance].[dbo].[tbAttendance] set isNotification='1' where EmployeeNo='" + columns[0].value + "' and WriteDate='" + columns[7].value + "' and WriteTime='" + columns[8].value + "'" )
        .execute()
        .then(function(rowCount) {
            console.log(rowCount);
        }).fail(function(err) {
            console.log(err);
        });

    
}

function executeStatement() {  
    request = new Request("SELECT top 3 [Attendance].[dbo].[tbAttendance].EmployeeNo,EmployeeName,EnEmployeeName,CardType,CardDate,CardTime,deviceToken,WriteDate,WriteTime from [Attendance].[dbo].[tbAttendance] inner join [Attendance].[dbo].[EmpApnMapping] " +
                          "on [Attendance].[dbo].[tbAttendance].EmployeeNo=[Attendance].[dbo].[EmpApnMapping].EmployeeNo where isNotification = '0'", function(err) {  
    if (err) {
        console.log(err);}  
    });  
    var result = "";  
    request.on('row', function(columns) {  
        
        sendNotification(columns[0].value,columns[1].value,columns[3].value,columns[4].value,columns[5].value,columns[6].value);
        
        updateSql(columns);
    });  

    request.on('done', function(rowCount, more) {  
    console.log(rowCount + ' rows returned');  
    });  
    connection.execSql(request);  

    setTimeout(executeStatement, 3000);
}  

function sendNotification(EmployeeNo,EmployeeName,CardType,CardDate,CardTime,deviceId)
{
    var apn = require('apn');

    // Set up apn with the APNs Auth Key
    var apnProvider = new apn.Provider({  
        token: {
            key: 'apns.p8', // Path to the key p8 file
            keyId: '922TAN4H44', // The Key ID of the p8 file (available at https://developer.apple.com/account/ios/certificate/key)
            teamId: 'GD9579ZT9J', // The Team ID of your Apple Developer Account (available at https://developer.apple.com/account/#/membership/)
        },
        production: true // Set to true if sending a notification to a production iOS app
    });

    // Enter the device token from the Xcode console
    var deviceToken = String(deviceId);

    // Prepare a new notification
    var notification = new apn.Notification();

    // Specify your iOS app's Bundle ID (accessible within the project editor)
    notification.topic = 'Regalscan.com.RegalAttendance';

    // Set expiration to 1 hour from now (in case device is offline)
    notification.expiry = Math.floor(Date.now() / 1000) + 3600;

    // Set app badge indicator
    notification.badge = 0;

    // Play ping.aiff sound when the notification is received
    notification.sound = 'ping.aiff';

    // Display the following message (the actual notification text, supports emoji)
    var CardTypeCN;
    if(String(CardType) === String("1")) CardTypeCN = "上班";
    else if(String(CardType) === String("2")) CardTypeCN = "下班";
    else if(String(CardType) === String("3")) CardTypeCN = "加班上班";
    else if(String(CardType) === String("4")) CardTypeCN = "加班下班";


    //notification.alert = '工號:' + EmployeeNo + " 姓名:" + EmployeeName + ' 已於 ' + CardDate + ' ' + CardTime + ' 打了' + CardTypeCN + '卡 \u270C';
    notification.alert = '帝商最漂亮的陳weiwei終於到最後一分鐘了，離開時不要回頭，真接走出去，祝你新工作順心如意，!!\u270C';
    // Send any extra payload data with the notification which will be accessible to your app in didReceiveRemoteNotification
    notification.payload = {id: 123};

    // Actually send the notification
    apnProvider.send(notification, deviceToken).then(function(result) {  
        // Check the result for any failed devices
        console.log(result);
    });
}

