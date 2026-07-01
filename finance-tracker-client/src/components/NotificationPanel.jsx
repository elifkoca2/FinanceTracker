export default function NotificationPanel({ onClose }) {
    return (
      <div className="absolute right-0 top-10 w-80 bg-white rounded-xl shadow-xl border border-gray-200 z-50">
        <div className="flex items-center justify-between px-4 py-3 border-b">
          <h3 className="font-semibold text-gray-800">Bildirimler</h3>
          <button onClick={onClose} className="text-gray-400 hover:text-gray-600">✕</button>
        </div>
        <div className="py-8 text-center text-gray-400 text-sm">
          Bildirimler yükleniyor...
        </div>
      </div>
    );
  }