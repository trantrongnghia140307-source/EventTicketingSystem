# Web API Test Guide

Base URL: `http://localhost:5139`

## 1. Xem sự kiện
`GET /api/events`

## 2. Mua vé VIP không voucher
`POST /api/buy-ticket`
```json
{
  "eventId": "EVT001",
  "tierName": "VIP Tier",
  "voucherCode": null
}
```

## 3. Mua vé có voucher
```json
{
  "eventId": "EVT001",
  "tierName": "General Tier",
  "voucherCode": "HCMUTE10"
}
```

## 4. Voucher sai
Dùng một mã bất kỳ không có trong hệ thống -> API trả lỗi.

## 5. Check-in
```json
{
  "ticketCode": "<ticketCode nhận được khi mua vé>"
}
```
Gửi lại đúng request lần 2 -> API phải từ chối vì ticket đã check-in.
