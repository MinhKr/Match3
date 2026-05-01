## Những diều cần nhớ

* ###### Sirenix : odin pro mất phí , lấy từ prj mẫu
* ###### DOTween
* ###### Grid Manager : Là trung tâm quản lý level

         - Sinh brick / group brick theo dữ liệu level



         - Gán type + sprite (concept) cho từng brick



         - Shuffle brick để tránh pattern cố định



         - Kiểm tra chồng layer (overlap)



         - Lưu danh sách toàn bộ brick trong level

      => Có thể xem đây là Level Controller + Brick Spawner + Rule Checker

* ###### List Item .cs : điều khiển toàn bộ hệ thống Item / Booster trong UI gameplay, bao gồm

 	- Undo : mô hình Stack?



 	- Merge



 	- Shuffle



 	- Tile Return



 	- Add Slot



######  	*Nó chịu trách nhiệm:*



 		- Bật / tắt item theo level



 		- Quyết định dùng coin hay xem quảng cáo



 		- Kiểm tra item có dùng được không



 		- Gọi logic xử lý item trong GridManager, ListItemPicked, GameManager

 

