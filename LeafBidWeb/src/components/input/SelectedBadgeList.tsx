import React from "react";
import { Badge } from "react-bootstrap";
import { Product } from "@/types/Product";
import s from "./SelectedBadgeList.module.css";

interface SelectedBadgeListProps {
    items: Product[];
    onRemove: (product: Product) => void;
}

/**
 * Displays selected products as removable badges.
 */
const SelectedBadgeList: React.FC<SelectedBadgeListProps> = ({ items, onRemove }) => {
    if (items.length === 0) return null;

    return (
        <div className={`mb-3 d-flex flex-wrap gap-2 ${s.badgeContainer}`}>
            {items.map((p, i) => (
                <Badge
                    key={p.productId}
                    bg="secondary"
                    pill
                    className={s.selectedBadge}
                    title={`Click to remove ${p.productName}`}
                    onClick={() => onRemove(p)}
                >
                    {i + 1}. {p.productName}
                </Badge>
            ))}
        </div>
    );
};

export default SelectedBadgeList;
