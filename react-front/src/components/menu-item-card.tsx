import type { MenuItemCardData } from "../types/menu-item";

interface MenuItemCardProps {
  item: MenuItemCardData & { id?: number };
  showOrderButton?: boolean;
  onDelete?: (id: number) => void;
  deleting?: boolean;
}

export function MenuItemCard({
  item,
  showOrderButton = true,
  onDelete,
  deleting,
}: MenuItemCardProps) {
  const itemId = item.id;

  return (
    <div className="menu-item-content">
      <div className="menu-item-img-container">
        <img src={item.imageUrl} alt={item.name} />
      </div>
      <h3>{item.name}</h3>
      <p>
        {item.price}
        <span>$</span>
      </p>
      <p className="menu-description">{item.description}</p>
      {showOrderButton && (
        <button type="button" className="btn-order">
          Order delivery
          <svg
            width="18"
            height="11"
            viewBox="0 0 18 11"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path d="M14.8035 1.72707C14.8035 0.899239 14.0532 0.221924 13.1363 0.221924H10.6355V1.72707H13.1363V3.72138L10.2354 6.99507H7.3011V3.23221H3.96671C2.12446 3.23221 0.632324 4.57932 0.632324 6.2425V8.50022H2.29952C2.29952 9.74949 3.41654 10.7579 4.80031 10.7579C6.18408 10.7579 7.3011 9.74949 7.3011 8.50022H11.0356L14.8035 4.24819V1.72707ZM4.80031 9.25279C4.34183 9.25279 3.96671 8.91413 3.96671 8.50022H5.63391C5.63391 8.91413 5.25879 9.25279 4.80031 9.25279Z" />
            <path d="M7.30106 0.974487H3.13306V2.47961H7.30106V0.974487Z" />
            <path d="M14.8035 6.24249C13.4198 6.24249 12.3027 7.25093 12.3027 8.5002C12.3027 9.74946 13.4198 10.7579 14.8035 10.7579C16.1873 10.7579 17.3043 9.74946 17.3043 8.5002C17.3043 7.25093 16.1873 6.24249 14.8035 6.24249ZM14.8035 9.25277C14.345 9.25277 13.9699 8.91411 13.9699 8.5002C13.9699 8.08629 14.345 7.74763 14.8035 7.74763C15.262 7.74763 15.6371 8.08629 15.6371 8.5002C15.6371 8.91411 15.262 9.25277 14.8035 9.25277Z" />
          </svg>
        </button>
      )}
      {onDelete && itemId != null && (
        <button
          type="button"
          className="btn-delete"
          onClick={() => onDelete(itemId)}
          disabled={deleting}
        >
          {deleting ? "Deleting..." : "Delete"}
        </button>
      )}
    </div>
  );
}
