public class ELocalNotificationType
{
    /* 기존 등록된 알람을 취소후 신규 등록 */
    public const int CANCEL_CURRENT         = 1;

    /* 기존 등록된 알람 반환 */
    public const int NO_CREATE              = 2;

    /* 1회용 알람 등록 */
    public const int ONE_SHOT               = 3;

    /* 기존 등록된 알람을 갱신 */
    public const int UPDATE_CURRENT         = 4;
}