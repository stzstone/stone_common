package com.sundaytoz.plugins.common.receivers;

import com.sundaytoz.plugins.common.utils.NotificationUtil;
import com.sundaytoz.plugins.common.utils.StorageUtil;
import android.annotation.TargetApi;
import android.app.Notification;
import android.app.NotificationManager;
import android.app.NotificationChannel;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.res.Resources;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.graphics.Paint;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.provider.Settings;
import androidx.core.app.NotificationCompat;
import androidx.core.app.NotificationCompat.Builder;
import androidx.core.app.NotificationManagerCompat;
import android.text.Html;
import android.util.Log;
import android.widget.RemoteViews;
import java.util.Map;
import java.util.regex.Pattern;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.URL;

class StzNotificationTask extends AsyncTask<String, Void, Bitmap> 
{    
    private static final String LOG_TAG = "Sundaytoz";
    private Exception exception;    
    private Context context;
    private Intent intent;

    private static int getResourcesColor(Resources paramResources, int paramInt)
    {
        if (Build.VERSION.SDK_INT < 23) {}
        for (paramInt = paramResources.getColor(paramInt);; paramInt = getResourcesColorNew(paramResources, paramInt)) {
            return paramInt;
        }
    }
    
    @TargetApi(23)
    private static int getResourcesColorNew(Resources paramResources, int paramInt)
    {
        return paramResources.getColor(paramInt, null);
    }   

    // adapted from post by Phil Haack and modified to match better
    public final static String tagStart=
            "\\<\\w+((\\s+\\w+(\\s*\\=\\s*(?:\".*?\"|'.*?'|[^'\"\\>\\s]+))?)+\\s*|\\s*)\\>";
    public final static String tagEnd=
            "\\</\\w+\\>";
    public final static String tagSelfClosing=
            "\\<\\w+((\\s+\\w+(\\s*\\=\\s*(?:\".*?\"|'.*?'|[^'\"\\>\\s]+))?)+\\s*|\\s*)/\\>";
    public final static String htmlEntity=
            "&[a-zA-Z][a-zA-Z0-9]+;";
    public final static Pattern htmlPattern=Pattern.compile(
            "("+tagStart+".*"+tagEnd+")|("+tagSelfClosing+")|("+htmlEntity+")",
            Pattern.DOTALL
    );

    /**
     * Will return true if s contains HTML markup tags or entities.
     *
     * @param s String to test
     * @return true if string contains HTML
     */
    public static boolean isHtml(String s) {
        boolean ret=false;
        if (s != null) {
            ret=htmlPattern.matcher(s).find();
        }
        return ret;
    }
    private CharSequence convertHtmlToCharsequence(String text)
    {
        if (android.os.Build.VERSION.SDK_INT < android.os.Build.VERSION_CODES.N)
        {
            // noinspection deprecation
            return Html.fromHtml(text);
        }
        return Html.fromHtml(text, Html.FROM_HTML_MODE_LEGACY);
    }

    private CharSequence convertRawToString(String rawString, boolean isBold )
    {
        CharSequence retMessage = "";
        String bundleMessage = rawString;

        boolean isHtmlTag = isHtml(bundleMessage);

        Log.d(LOG_TAG, "NotificationReceiver::onReceive isHtmlTag :" + isHtmlTag );
        if( isHtmlTag )
        {
            retMessage = convertHtmlToCharsequence(bundleMessage);
        }
        else
        {
            bundleMessage = bundleMessage.replace("\\r\\n", "\r\n");
            bundleMessage = bundleMessage.replace("\\n", "\n");
            bundleMessage = bundleMessage.replace("\\r", "\r");
            bundleMessage = bundleMessage.replace(".", "\n");

            retMessage = bundleMessage;
        }
        if( isBold && isHtmlTag == false )
        {
            retMessage = convertHtmlToCharsequence(("<b>" + bundleMessage +"</b>"));
        }
        return retMessage;
    }

    public void SetNotiParams(Context context, Intent intent )
    {
        this.context = context;
        this.intent = intent;
    }

    public NotificationChannel CreateNotiChannel()
    {
        Resources localResources = context.getResources();
        int app_name_id = localResources.getIdentifier("app_name", "string", context.getPackageName());
        CharSequence name = context.getString(app_name_id);

        final StringBuilder sb = new StringBuilder(name.length());
        sb.append(name);
        String oldId = sb.toString();
        // String newId = sb.toString() + "_DEFAULT";

        NotificationManager notificationManager = context.getSystemService(NotificationManager.class);
        // notificationManager.deleteNotificationChannel(oldId);

        NotificationChannel newNoti = notificationManager.getNotificationChannel(oldId);
        if(newNoti == null) {
            Log.d(LOG_TAG, "NotificationReceiver::CreateNotiChannel()->IMPORTANCE_MAX" );
            int importance = NotificationManager.IMPORTANCE_MAX;
            newNoti = new NotificationChannel(oldId, name, importance);
            newNoti.enableLights(true);
            newNoti.setLightColor(Color.RED);
            newNoti.enableVibration(true);
            newNoti.setVibrationPattern(new long[]{100, 200, 300, 400, 500, 400, 300, 200, 400});
        }

        return newNoti;
    }

    public Intent getNotificationIntent(String app_param, String deeplink)
    {
        Log.d(LOG_TAG, "StzNotificationTask::getNotificationIntent()->deeplink:" + deeplink );
        String className = StorageUtil.getMainActivityName(context);
        if(className == null || className.isEmpty()){
            className = "com.unity3d.player.UnityPlayerActivity";
        }
        Log.d(LOG_TAG, "[StzCommon] className " + className );
                
        Intent notificationIntent = null;
        try{
            notificationIntent = new Intent(context, Class.forName(className));
        }catch(Exception e){
            Log.d(LOG_TAG, "[StzCommon] Exception " + e.getMessage() );
            return null;
        }
        
        notificationIntent.putExtra("app_param", app_param);
        notificationIntent.putExtra("deeplink", deeplink);
        notificationIntent.addFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP | Intent.FLAG_ACTIVITY_CLEAR_TOP);
        notificationIntent.addCategory("android.intent.category.LAUNCHER");
        notificationIntent.setAction("android.intent.action.MAIN");

        return notificationIntent;
    }

    protected Bitmap doInBackground(String... urls) 
    {
        try {
            if( urls.length < 0 || urls[0] == null )
                return null;
            URL url = new URL(urls[0]);
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setDoInput(true);
            connection.connect();
            InputStream input = connection.getInputStream();
            Bitmap myBitmap = BitmapFactory.decodeStream(input);
            return myBitmap;
        } catch (Exception e) {
            this.exception = e;
            return null;
        } finally {
        }
    }

    private void createExpandedNoti(Bitmap bitmap)
    {
        Log.v(LOG_TAG, "createExpandedNoti");
        
        String channelId = "";
        
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) 
        {
            NotificationChannel mChannel = CreateNotiChannel();

            NotificationManager notificationManager = context.getSystemService(NotificationManager.class);

            notificationManager.createNotificationChannel(mChannel);
            channelId = mChannel.getId();
        }

        int noti_icon_id = GetNotificationId(context);
        String title = GetTitle(intent);
        String message = GetMessage(intent);
        int alarmId = GetAlramId(intent);
        String app_param = GetAppParam(intent);
        String deeplink = GetDeeplink(intent);

        Intent notificationIntent = getNotificationIntent(app_param, deeplink);
        if( notificationIntent == null )
            return;

        if(containPercentSymbol(title, message))
        {
            Log.d(LOG_TAG, "[StzCommon] createExpandedNoti : Percent Symbol Found! 퍼센트 기호가 있어요." );
            return;
        }

        PendingIntent contentIntent = PendingIntent.getActivity(context.getApplicationContext(), alarmId, notificationIntent, PendingIntent.FLAG_ONE_SHOT );

        NotificationCompat.Builder notiBuilder = new NotificationCompat.Builder(context, channelId);
        notiBuilder.setSmallIcon(noti_icon_id)
                .setLargeIcon(bitmap)
                .setContentTitle(title)
                .setContentText(message)
                .setStyle(new NotificationCompat.BigPictureStyle()
                .bigPicture(bitmap))
                .setContentIntent(contentIntent)
                .setAutoCancel(true);
        
        Notification notification = notiBuilder.build();

        NotificationManagerCompat notificationManager = NotificationManagerCompat.from(context);
        notificationManager.cancel(alarmId);
        notificationManager.notify(alarmId, notification);
    }

    protected void onPostExecute(Bitmap bitmap) {
        Log.v(LOG_TAG, "onPostExecute");

        if( Build.VERSION.SDK_INT < Build.VERSION_CODES.JELLY_BEAN || this.exception != null || bitmap == null )
        {
            Log.v("main", "Proceed custom notification-> DeviceVersion : " + Build.VERSION.SDK_INT
            + ", Exception : " + this.exception == null ? "null" :this.exception.getMessage()
            + ", bitmap is " + bitmap == null ? "null" : "not null");
            
            createCumstomNoti();
        }
        else
        {
            createExpandedNoti(bitmap);    
        }
    }

    private int GetNotificationId(Context context)
    {
        Resources localResources = context.getResources();
     
        int noti_icon_id = 0;

        if (Build.VERSION.SDK_INT >= 21)
        {
            noti_icon_id = localResources.getIdentifier("ic_notif_white", "drawable", context.getPackageName());
        }

        if (noti_icon_id==0)
        {
            noti_icon_id = localResources.getIdentifier("ic_notif", "drawable", context.getPackageName());
        }

        if (noti_icon_id==0)
        {
            noti_icon_id = localResources.getIdentifier("app_icon", "drawable", context.getPackageName());
        }
        
        return noti_icon_id;
    }

    private final String emo_regex = "(?:[\uD83C\uDF00-\uD83D\uDDFF]|[\uD83E\uDD00-\uD83E\uDDFF]|" +
            "[\uD83D\uDE00-\uD83D\uDE4F]|[\uD83D\uDE80-\uD83D\uDEFF]|" +
            "[\u2600-\u26FF]\uFE0F?|[\u2700-\u27BF]\uFE0F?|\u24C2\uFE0F?|" +
            "[\uD83C\uDDE6-\uD83C\uDDFF]{1,2}|" +
            "[\uD83C\uDD70\uD83C\uDD71\uD83C\uDD7E\uD83C\uDD7F\uD83C\uDD8E\uD83C\uDD91-\uD83C\uDD9A]\uFE0F?|" +
            "[\u0023\u002A\u0030-\u0039]\uFE0F?\u20E3|[\u2194-\u2199\u21A9-\u21AA]\uFE0F?|[\u2B05-\u2B07\u2B1B\u2B1C\u2B50\u2B55]\uFE0F?|" +
            "[\u2934\u2935]\uFE0F?|[\u3030\u303D]\uFE0F?|[\u3297\u3299]\uFE0F?|" +
            "[\uD83C\uDE01\uD83C\uDE02\uD83C\uDE1A\uD83C\uDE2F\uD83C\uDE32-\uD83C\uDE3A\uD83C\uDE50\uD83C\uDE51]\uFE0F?|" +
            "[\u203C\u2049]\uFE0F?|[\u25AA\u25AB\u25B6\u25C0\u25FB-\u25FE]\uFE0F?|" +
            "[\u00A9\u00AE]\uFE0F?|[\u2122\u2139]\uFE0F?|\uD83C\uDC04\uFE0F?|\uD83C\uDCCF\uFE0F?|" +
            "[\u231A\u231B\u2328\u23CF\u23E9-\u23F3\u23F8-\u23FA]\uFE0F?)";
    private static final Paint mPaint = new Paint();
    private static float unknownMeasure = 0.0f;
    private static final String unknown = "\u1fff";

    private boolean isAvailableString(String oneCharacter)
    {
        if( unknownMeasure == 0.0f )
            unknownMeasure = mPaint.measureText(unknown);

        boolean isEmoji = oneCharacter.matches(emo_regex);

        if( isEmoji == true )
        {
            float measure = mPaint.measureText(oneCharacter);

            return measure != unknownMeasure;
        }
        else
        {
            return true;
        }
    }

    private String replaceReservedWord(String source)
    {
        Log.v(LOG_TAG, "replaceReservedWord()->begin source : " + source);
        
        if( this.context == null )
            return source;

        Map<String, ?> reservedWordMap = StorageUtil.getlAllReservedWord(this.context);
        
        if ( reservedWordMap != null && reservedWordMap.isEmpty() == false )
        {
            for (Map.Entry<String, ?> entry : reservedWordMap.entrySet()) {

                String key = "%" + entry.getKey() +"%";
                String value = (String)entry.getValue();

                Log.v(LOG_TAG, "replaceReservedWord()->key : " + key + ", value : " + value);

                if( source.contains(key) )
                    source = source.replace(key, value);
            }
        }

        Log.v(LOG_TAG, "replaceReservedWord()->end source : " + source);
        
        return source;
    }

    private String replaceUnavailableEmoji(String source, String delimeter)
    {
        StringBuilder sb = new StringBuilder();
        int index = 0;

        while ( index < source.length())
        {
            int codePoint = source.codePointAt(index);
            index += Character.charCount(codePoint);

            String singleCharacter =new String(Character.toChars(codePoint));

            if( isAvailableString(singleCharacter))
            {
                sb.append(singleCharacter);
            }
            else
            {
                sb.append(delimeter);
            }
        }
        //Log.v(LOG_TAG, "StzNotificationTask::replaceUnavailableEmoji()-> ret : " + sb.toString());

        return sb.toString();
    }

    private String replaceText(String source, String delimeter)
    {
        String ret = replaceReservedWord(source);

        ret = replaceUnavailableEmoji(ret, delimeter);

        return ret;
    }

    private String GetTitle(Intent intent)
    {
        Bundle bundle = intent.getExtras();

        CharSequence title = convertRawToString(bundle.getString("title"), true );
        
        if (title==null)
        {
            title = "";
        }
        
        String ret = replaceText(title.toString(), "");
        
        return ret;
    }
    private String GetMessage(Intent intent)
    {
        Bundle bundle = intent.getExtras();

        CharSequence message = convertRawToString(bundle.getString("message"), false );
        
        if (message==null)
        {
            message = "";
        }

        String ret = replaceText(message.toString(), "");
        
        return ret;
    }
    private int GetAlramId(Intent intent)
    {
        Bundle bundle = intent.getExtras();

        return bundle.getInt("id");        
    }
    private String GetAppParam(Intent intent)
    {
        Bundle bundle = intent.getExtras();
        return bundle.getString("app_param");
    }
    private String GetDeeplink(Intent intent)
    {
        Bundle bundle = intent.getExtras();
        return bundle.getString("deeplink");
    }
    private Boolean containPercentSymbol( String title, String message )
    {
        Pattern pattern = Pattern.compile("%[a-zA-Z]+%");
        
        boolean isContainPercent = pattern.matcher(title).find() || pattern.matcher(message).find();

        return isContainPercent;
    }
    public void createCumstomNoti()
    {
        if ( this.context == null || this.intent == null )
            return;

        Resources localResources = context.getResources();
        Builder notiBuilder = null;
        int large_icon_id = 0;
        CharSequence title = "";
        CharSequence message = "";
        String app_param = "";
        String deeplink = "";
        Bundle bundle = intent.getExtras();
        int alarmId = 0;
        int notiType = 0;

        int title_color = 0;    //  "color_notif_text";
        int msg_color = 0;      //  "color_notif_title";
        int title_size  = 18;
        int msg_size    = 13;

        String bg_image_name    = "notif_background";
        
        try
        {
            Map<String,String> savedNotiInfo = StorageUtil.getNotificationInfo(context);

            title = GetTitle(intent);
            message = GetMessage(intent);

            alarmId = GetAlramId(intent);
            notiType    = bundle.getInt("notiType");
            
            if (title == "" && message == ""){
                return;
            }

            if(containPercentSymbol(title.toString(), message.toString()))
            {
                Log.d(LOG_TAG, "[StzCommon] Exception : Percent Symbol Found! 퍼센트 기호가 있어요." );
                return;
            }
            
            String tmp      = null;
            
            app_param   = GetAppParam(intent);

            deeplink = GetDeeplink(intent);

            tmp         = bundle.getString("bg_image_name");
            if( !tmp.isEmpty()){
                bg_image_name = tmp;
            }
            
            tmp         = bundle.getString("msg_color");
            if( tmp == null || tmp.isEmpty()){
                tmp = savedNotiInfo.get("msg_color");
            }
            if( tmp != null && !tmp.isEmpty()){
                msg_color = Color.parseColor(tmp);
            }
            
            tmp         = bundle.getString("title_color");
            if( tmp == null || tmp.isEmpty()){
                tmp = savedNotiInfo.get("title_color");
            }           
            if( tmp != null && !tmp.isEmpty()){
                title_color = Color.parseColor(tmp);
            }
            
            int noti_icon_id = GetNotificationId(context);

            large_icon_id = localResources.getIdentifier("ic_notif_large", "drawable", context.getPackageName());
        
            if(noti_icon_id==0)
            {
                large_icon_id = localResources.getIdentifier("app_icon", "drawable", context.getPackageName()); 
            }

            notiBuilder = new NotificationCompat.Builder(context)
                    .setSmallIcon(noti_icon_id)
                    .setContentTitle(title)
                    .setContentText(message)
                    .setTicker(title)
                    .setAutoCancel(true)                    
                    .setDefaults(1);
        }
        catch(Exception ex)
        {
            Log.d(LOG_TAG, "[StzCommon] Exception " + ex.getMessage() );
            ex.printStackTrace();
            return;
        }

        if (large_icon_id!=0){
            notiBuilder.setLargeIcon( BitmapFactory.decodeResource(context.getResources(), large_icon_id) );
            Log.d(LOG_TAG, "[StzCommon] setLargeIcon " + large_icon_id);
        }

        if (Build.VERSION.SDK_INT >= 21)
        {
            int color_notif_bkg = localResources.getIdentifier("color_notif_bkg", "color", context.getPackageName());
            if (color_notif_bkg != 0) {
                notiBuilder.setColor(getResourcesColor(localResources, color_notif_bkg));
            }
            Log.d(LOG_TAG, "[StzCommon] color_notif_bkg " + color_notif_bkg);
        }

        int notification_layout_id  = localResources.getIdentifier("notification_default", "layout", context.getPackageName());
        Log.d(LOG_TAG, "[StzCommon] notification_layout_id " + notification_layout_id);
        
        int notif_background_id     = localResources.getIdentifier(bg_image_name, "drawable", context.getPackageName());
        Log.d(LOG_TAG, "[StzCommon] notif_background_id " + notif_background_id);
        
        RemoteViews remote_view = new RemoteViews(context.getPackageName(), notification_layout_id);
        if (notif_background_id!=0){
            int show_bg = localResources.getIdentifier("notif_background", "id", context.getPackageName());
            if (show_bg != 0) {
                remote_view.setImageViewBitmap(show_bg, BitmapFactory.decodeResource(context.getResources(), notif_background_id));
            }
            Log.d(LOG_TAG, "[StzCommon] show_bg " + show_bg);
        }
        
        if (large_icon_id!=0){
            int notif_image_id = localResources.getIdentifier("notif_image", "id", context.getPackageName());
            if (notif_image_id != 0) {
                remote_view.setImageViewBitmap(notif_image_id, BitmapFactory.decodeResource(context.getResources(), large_icon_id));
            }
            Log.d(LOG_TAG, "[StzCommon] notif_image_id " + notif_image_id);
        }       
        int notif_title_id = localResources.getIdentifier("notif_title", "id", context.getPackageName());
        if (notif_title_id != 0)
        {
            remote_view.setTextViewText(notif_title_id, title);
            if (title_color!=0){
                remote_view.setTextColor(notif_title_id, title_color);
            }else{
                int color_notif_title = localResources.getIdentifier("color_notif_title", "color", context.getPackageName());
                if (color_notif_title != 0) {
                    remote_view.setTextColor(notif_title_id, getResourcesColor(localResources, color_notif_title));
                }
            }
            if( title_size != 0 )
            {
                Log.d(LOG_TAG, "title_size " + title_size);
                remote_view.setTextViewTextSize(notif_title_id, android.util.TypedValue.COMPLEX_UNIT_DIP, title_size);
            }

            Log.d(LOG_TAG, "[StzCommon] color_notif_title " + title_color);
        }
        
        int notif_message_id = localResources.getIdentifier("notif_message", "id", context.getPackageName());
        if (notif_message_id != 0)
        {
            remote_view.setTextViewText(notif_message_id, message);
            if (msg_color!=0){
                remote_view.setTextColor(notif_message_id, msg_color);
            }else{
                int color_notif_text = localResources.getIdentifier("color_notif_text", "color", context.getPackageName());
                if (color_notif_text != 0) {
                    remote_view.setTextColor(notif_message_id, getResourcesColor(localResources, color_notif_text));
                }
            }           
            if( msg_size != 0 )
            {
                Log.d(LOG_TAG, "[StzCommon] msg_size " + msg_size);
                remote_view.setTextViewTextSize(notif_message_id, android.util.TypedValue.COMPLEX_UNIT_DIP, msg_size);
            }
            
            Log.d(LOG_TAG, "[StzCommon] color_notif_text " + msg_color);
        }
        notiBuilder.setContent(remote_view);
        notiBuilder.setStyle(null);

        String ns = Context.NOTIFICATION_SERVICE;
        NotificationManager notificationManager = (NotificationManager)context.getSystemService(ns);
        
        Intent notificationIntent = getNotificationIntent(app_param, deeplink);
        if( notificationIntent == null )
            return;

        PendingIntent contentIntent = PendingIntent.getActivity(context.getApplicationContext(), alarmId, notificationIntent, PendingIntent.FLAG_ONE_SHOT );

        notiBuilder.setContentIntent(contentIntent);
        notificationManager.cancel(alarmId);

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O)
        {
            NotificationChannel mChannel = CreateNotiChannel();

            notificationManager.createNotificationChannel(mChannel);
            
            notiBuilder.setChannelId(mChannel.getId());
        }

        notificationManager.notify(alarmId, notiBuilder.build());

        // 저장된 노티 정보 제거 
        int pendingType = NotificationUtil.toPendingType(notiType);
        NotificationUtil.removeAlarmInfoFromSahredPref(context, alarmId, pendingType);
    }
}